using System.Net.Http.Headers;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Infrastructure.BackgroundJobs;
using RecruitPro.Infrastructure.Email;
using RecruitPro.Infrastructure.Files;
using RecruitPro.Infrastructure.Identity;
using RecruitPro.Infrastructure.Persistence;
using RecruitPro.Infrastructure.Persistence.Interceptors;

namespace RecruitPro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Scoped, not Singleton: AuditableEntitySaveChangesInterceptor depends on the scoped
        // ICurrentUserService (which reads the current HttpContext).
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<DomainEventDispatchInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(
                sp.GetRequiredService<SoftDeleteInterceptor>(),
                sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>(),
                sp.GetRequiredService<DomainEventDispatchInterceptor>());
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.Configure<SupabaseStorageOptions>(configuration.GetSection(SupabaseStorageOptions.SectionName));
        services.AddHttpClient<IFileStorageService, SupabaseStorageFileStorageService>((sp, client) =>
        {
            var storageOptions = sp.GetRequiredService<IOptions<SupabaseStorageOptions>>().Value;
            client.BaseAddress = new Uri(storageOptions.Url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storageOptions.ServiceRoleKey);
        });

        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.SectionName));
        services.AddHttpClient<IEmailService, SendGridEmailService>((sp, client) =>
        {
            var sendGridOptions = sp.GetRequiredService<IOptions<SendGridOptions>>().Value;
            client.BaseAddress = new Uri("https://api.sendgrid.com");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sendGridOptions.ApiKey);
        });

        // Same PostgreSQL database as the application data, per the blueprint's "no separate
        // infra" decision for background jobs at this stage.
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(configuration.GetConnectionString("Default"))));
        services.AddHangfireServer();
        services.AddScoped<ProcessPendingNotificationsJob>();

        return services;
    }
}
