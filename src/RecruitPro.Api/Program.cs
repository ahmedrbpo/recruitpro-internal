using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Middleware;
using RecruitPro.Application;
using RecruitPro.Infrastructure;
using RecruitPro.Infrastructure.BackgroundJobs;
using RecruitPro.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RecruitPro API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter a valid JWT access token.",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            []
        },
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// AllowCredentials requires an explicit origin list (not AllowAnyOrigin) because the frontend's
// refresh flow relies on the httpOnly refresh-token cookie, which browsers only attach to
// cross-origin requests when credentialed CORS is set up this way.
// Origins come from config (Cors:AllowedOrigins), not a hardcoded literal, so adding the
// production frontend's domain is a host config change, not a code redeploy.
const string FrontendCorsPolicy = "Frontend";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["https://localhost:5173"];
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// Read lazily inside the options callback (not eagerly here): WebApplicationFactory's test
// config overrides are only merged into builder.Configuration during builder.Build(), so an
// eager read at this point would miss them and see only "real" config sources.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt configuration section is missing.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
        };
    });

// Per CLAUDE.md: per-IP sliding window on auth endpoints (stricter — brute-force surface),
// per-user token bucket on general API endpoints (falls back to per-IP for anonymous callers,
// e.g. before UseAuthentication runs or on public endpoints).
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var userId = context.User.Identity?.IsAuthenticated == true
            ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            : null;
        var partitionKey = userId is not null ? $"user:{userId}" : $"ip:{context.Connection.RemoteIpAddress}";

        return RateLimitPartition.GetTokenBucketLimiter(partitionKey, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 100,
            TokensPerPeriod = 20,
            ReplenishmentPeriod = TimeSpan.FromSeconds(10),
            QueueLimit = 0,
            AutoReplenishment = true,
        });
    });

    options.AddPolicy("AuthPolicy", context =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 4,
                QueueLimit = 0,
            }));
});

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    // Fail closed: any endpoint without an explicit [AllowAnonymous] or [RequirePermission]
    // requires at least a valid JWT by default, instead of being silently public if a future
    // controller action forgets to add an authorization attribute.
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

var app = builder.Build();

// Checked after Build(), not before: app.Configuration is the same ConfigurationManager as
// builder.Configuration, but only fully merged (including any test-host overrides) by this point.
if (string.IsNullOrEmpty(app.Configuration["Jwt:Secret"]))
{
    throw new InvalidOperationException(
        "Jwt:Secret is not configured. Set it via `dotnet user-secrets set \"Jwt:Secret\" \"<value>\"` " +
        "for local development, or the Jwt__Secret environment variable / Secrets Manager in other environments. " +
        "It must never be committed to appsettings.json.");
}

// Registered first so it wraps every other middleware, including error responses from
// ExceptionHandlingMiddleware further down — security headers should apply to those too.
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Enabled in every environment (not gated to Development): every endpoint requires a Bearer
// token to actually call per the FallbackPolicy above, so exposing the route/schema catalog
// here doesn't grant access to anything — it's the only way to discover the API surface without
// reading the source, since there's no browsable directory listing otherwise.
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();

// Serves the built React SPA from wwwroot (copied there at publish time). Same-origin with the
// API, not just same-site: the refresh-token cookie is SameSite=Strict, and Strict cookies are
// never delivered cross-site regardless of HTTPS/CORS — that's what broke the earlier separate
// Vercel deployment. Placed before UseAuthorization() so static assets bypass the endpoint
// routing/auth pipeline entirely rather than tripping the fail-closed FallbackPolicy below.
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(FrontendCorsPolicy);
app.UseAuthentication();

// After UseAuthentication (needs the ClaimsPrincipal populated to partition by user), before
// UseAuthorization (no reason a rate-limited-out caller should also pay for a permission check).
app.UseRateLimiter();

app.UseAuthorization();
app.MapControllers();

// Anonymous: client-side routes (e.g. /login, /jobs) have no matching controller action, so
// without this the fail-closed FallbackPolicy above would 401 the SPA shell before its own JS
// router ever got a chance to run.
app.MapFallbackToFile("index.html").AllowAnonymous();

app.MapHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireDashboardAuthorizationFilter()],
});

RecurringJob.AddOrUpdate<ProcessPendingNotificationsJob>(
    "process-pending-notifications",
    job => job.ExecuteAsync(CancellationToken.None),
    Cron.Minutely());

app.Run();

// Exposed for RecruitPro.Api.IntegrationTests' WebApplicationFactory<Program>.
public partial class Program;
