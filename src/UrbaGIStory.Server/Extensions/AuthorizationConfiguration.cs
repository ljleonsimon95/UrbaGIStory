namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for configuring authorization policies.
/// </summary>
public static class AuthorizationConfiguration
{
    /// <summary>
    /// Configures role-based authorization policies.
    /// </summary>
    public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Configure role-based authorization
            options.AddPolicy("TechnicalAdministrator", policy => 
                policy.RequireRole("TechnicalAdministrator"));
            options.AddPolicy("OfficeManager", policy => 
                policy.RequireRole("OfficeManager"));
            options.AddPolicy("Specialist", policy => 
                policy.RequireRole("Specialist"));
        });

        return services;
    }
}

