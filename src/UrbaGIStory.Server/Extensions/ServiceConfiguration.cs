using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for registering application services.
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// Registers application-specific services.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<BackupService>();
        services.AddScoped<LogsService>();
        services.AddScoped<PermissionService>();
        services.AddScoped<EntityService>();
        services.AddSingleton<PerformanceMetricsService>();

        return services;
    }
}

