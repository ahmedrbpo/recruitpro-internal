using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

// Registered first so it wraps every other middleware, including auth failures further down.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

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
