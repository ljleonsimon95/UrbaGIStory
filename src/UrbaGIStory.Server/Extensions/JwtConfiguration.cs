using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for configuring JWT authentication.
/// </summary>
public static class JwtConfiguration
{
    /// <summary>
    /// Configures JWT Bearer authentication.
    /// </summary>
    public static IServiceCollection AddJwtConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero, // Remove delay of token expiration
                // Map the "role" claim from JWT to ClaimTypes.Role for authorization
                RoleClaimType = "role",
                // Also set NameClaimType for consistency
                NameClaimType = ClaimTypes.Name
            };

            // Add event handlers for token validation
            options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();
                    
                    var claims = context.Principal?.Claims.Select(c => $"{c.Type}={c.Value}").ToList() ?? new List<string>();
                    var roles = context.Principal?.Claims
                        .Where(c => c.Type == "role" || c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList() ?? new List<string>();
                    
                    logger.LogInformation("JWT Token Validated - Claims: {Claims}", string.Join(", ", claims));
                    logger.LogInformation("JWT Token Validated - Roles: {Roles}", string.Join(", ", roles));
                    
                    // CRITICAL FIX: Create a new ClaimsIdentity with correct RoleClaimType
                    // The existing identity has RoleClaimType set incorrectly, so we need to recreate it
                    if (context.Principal != null)
                    {
                        var isInRole = context.Principal.IsInRole("TechnicalAdministrator");
                        logger.LogInformation("JWT Token Validated - IsInRole(TechnicalAdministrator): {IsInRole}", isInRole);
                        
                        var oldIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (oldIdentity != null)
                        {
                            // Create new identity with correct RoleClaimType
                            var newIdentity = new ClaimsIdentity(
                                oldIdentity.Claims,
                                oldIdentity.AuthenticationType,
                                oldIdentity.NameClaimType,
                                ClaimTypes.Role); // Set RoleClaimType here
                            
                            // Create new principal with the corrected identity
                            var newPrincipal = new ClaimsPrincipal(newIdentity);
                            context.Principal = newPrincipal;
                            
                            logger.LogInformation("Created new ClaimsIdentity with RoleClaimType: {RoleClaimType}", newIdentity.RoleClaimType);
                            
                            // Verify roles exist
                            var identityRoles = newIdentity.Claims
                                .Where(c => c.Type == ClaimTypes.Role)
                                .Select(c => c.Value)
                                .ToList();
                            logger.LogInformation("Roles found in new identity: {Roles}", string.Join(", ", identityRoles));
                            
                            // Test IsInRole after recreating identity
                            var testIsInRole = newPrincipal.IsInRole("TechnicalAdministrator");
                            logger.LogInformation("After recreating identity - IsInRole(TechnicalAdministrator): {IsInRole}", testIsInRole);
                        }
                    }
                    
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();
                    logger.LogError(context.Exception, "JWT Authentication Failed");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}

