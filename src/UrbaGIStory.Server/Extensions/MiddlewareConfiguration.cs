using UrbaGIStory.Server.Middleware;

namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for configuring middleware pipeline.
/// </summary>
public static class MiddlewareConfiguration
{
    /// <summary>
    /// Configures the HTTP request pipeline middleware.
    /// </summary>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Request logging middleware - must be early in pipeline
        app.UseMiddleware<RequestLoggingMiddleware>();

        // Global exception handler middleware - must be early in pipeline
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseCors("BlazorWasmPolicy");

        // Authentication must come before Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}

