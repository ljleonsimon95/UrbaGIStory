using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Data;

namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for configuring database and EF Core.
/// </summary>
public static class DatabaseConfiguration
{
    /// <summary>
    /// Configures EF Core with PostgreSQL and PostGIS.
    /// </summary>
    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
            );
            
            // Enable query logging for performance monitoring (only in development)
            // Note: Detailed query logging is handled by Serilog configuration
        });

        return services;
    }
}

