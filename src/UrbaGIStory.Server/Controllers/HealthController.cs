using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.Exceptions;
using UrbaGIStory.Server.Identity;

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

    /// <summary>
    /// DEBUG: Test login endpoint - verifies user exists and can authenticate
    /// </summary>
    [HttpPost("debug/test-login")]
    public async Task<IActionResult> TestLogin([FromBody] LoginRequest request)
    {
        using var scope = HttpContext.RequestServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Ok(new { 
                UserExists = false, 
                Message = $"User '{request.Username}' does not exist in database" 
            });
        }

        var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
        {
            return Ok(new { 
                UserExists = true, 
                PasswordValid = false,
                Message = "Password is incorrect" 
            });
        }

        var roles = await userManager.GetRolesAsync(user);
        
        return Ok(new
        {
            UserExists = true,
            PasswordValid = true,
            UserId = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Roles = roles.ToList(),
            Message = "User can authenticate successfully"
        });
    }

    /// <summary>
    /// DEBUG: Shows current user claims and roles (if authenticated)
    /// </summary>
    [HttpGet("debug/claims")]
    [Authorize] // Requiere autenticación pero no rol específico
    public IActionResult DebugClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        var isInRole = User.IsInRole("TechnicalAdministrator");
        var identityName = User.Identity?.Name ?? "NULL";
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        var roleClaims = User.Claims
            .Where(c => c.Type == "role" || c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        _logger.LogInformation("DEBUG Claims - IsAuthenticated: {IsAuth}, Name: {Name}, IsInRole(TechnicalAdministrator): {IsInRole}", 
            isAuthenticated, identityName, isInRole);
        _logger.LogInformation("DEBUG Claims - All Claims: {Claims}", 
            string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}")));
        _logger.LogInformation("DEBUG Claims - Role Claims: {Roles}", 
            string.Join(", ", roleClaims));

        return Ok(new
        {
            IsAuthenticated = isAuthenticated,
            IdentityName = identityName,
            IsInRole_TechnicalAdministrator = isInRole,
            AllClaims = claims,
            RoleClaims = roleClaims,
            AuthenticationType = User.Identity?.AuthenticationType
        });
    }
}

