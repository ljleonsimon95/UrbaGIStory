using Serilog;
using UrbaGIStory.Server.Extensions;

namespace UrbaGIStory.Server;

/// <summary>
/// Main entry point for the UrbaGIStory API application.
/// </summary>
public class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public static async Task Main(string[] args)
    {
        // Configure Serilog
        SerilogConfiguration.ConfigureSerilog();

        var builder = WebApplication.CreateBuilder(args);

        // Use Serilog for logging
        builder.Host.UseSerilog();

        var configuration = builder.Configuration;

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Configure services
        builder.Services.AddAuthorizationConfiguration();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddDatabaseConfiguration(configuration);
        builder.Services.AddIdentityConfiguration();
        builder.Services.AddJwtConfiguration(configuration);
        builder.Services.AddCorsConfiguration();
        builder.Services.AddApplicationServices();

        var app = builder.Build();

        // Seed initial data
        await app.SeedInitialDataAsync();

        // Configure middleware pipeline
        app.ConfigureMiddleware();

        // Run application
        try
        {
            Log.Information("Starting UrbaGIStory API");
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
