using Microsoft.AspNetCore.Identity;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.Identity;

namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for configuring ASP.NET Core Identity.
/// </summary>
public static class IdentityConfiguration
{
    /// <summary>
    /// Configures ASP.NET Core Identity with custom user and role types.
    /// </summary>
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings (using Identity defaults)
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
            
            // User settings
            options.User.RequireUniqueEmail = true;
            
            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}

