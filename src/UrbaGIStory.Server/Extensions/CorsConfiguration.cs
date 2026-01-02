namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for configuring CORS.
/// </summary>
public static class CorsConfiguration
{
    /// <summary>
    /// Configures CORS policy for Blazor WASM client.
    /// </summary>
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("BlazorWasmPolicy", policy =>
            {
                policy.WithOrigins("https://localhost:5001", "http://localhost:5000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return services;
    }
}

