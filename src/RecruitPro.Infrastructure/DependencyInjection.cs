using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RecruitPro.Application.Common.Interfaces;
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

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(
                sp.GetRequiredService<SoftDeleteInterceptor>(),
                sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
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

        return services;
    }
}
