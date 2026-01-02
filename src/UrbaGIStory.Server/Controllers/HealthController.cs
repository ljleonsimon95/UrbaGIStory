using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.Exceptions;

namespace UrbaGIStory.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(AppDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            // Test database connection
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                return Ok(new
                {
                    status = "error",
                    message = "Cannot connect to database",
                    postgisEnabled = false
                });
            }

            // Check PostGIS extension
            string? postgisVersion = null;
            try
            {
                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT PostGIS_version();";
                postgisVersion = await command.ExecuteScalarAsync() as string;
                await connection.CloseAsync();
            }
            catch
            {
                // PostGIS might not be enabled
                postgisVersion = null;
            }

            return Ok(new
            {
                status = "ok",
                message = "Database connection successful",
                postgisEnabled = !string.IsNullOrEmpty(postgisVersion),
                postgisVersion = postgisVersion ?? "Not available"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking database connection");
            return StatusCode(500, new
            {
                status = "error",
                message = ex.Message,
                postgisEnabled = false
            });
        }
    }

    /// <summary>
    /// Test endpoint for error handling - throws different exception types.
    /// Only available in Development environment.
    /// </summary>
    [HttpGet("test-error/{type}")]
    public IActionResult TestError(string type)
    {
        return type.ToLower() switch
        {
            "notfound" => throw new EntityNotFoundException("TestEntity", 123),
            "validation" => throw new ValidationException("Test validation error", new Dictionary<string, string[]>
            {
                { "field1", new[] { "Field1 is required" } },
                { "field2", new[] { "Field2 must be greater than 0" } }
            }),
            "argument" => throw new ArgumentException("Invalid argument provided"),
            "unauthorized" => throw new UnauthorizedAccessException("You are not authorized"),
            "generic" => throw new Exception("Generic exception for testing"),
            _ => BadRequest("Unknown error type. Use: notfound, validation, argument, unauthorized, generic")
        };
    }
}

