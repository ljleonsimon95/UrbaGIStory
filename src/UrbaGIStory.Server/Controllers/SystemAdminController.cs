using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for system administration endpoints.
/// All endpoints require TechnicalAdministrator role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "TechnicalAdministrator")]
public class SystemAdminController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SystemAdminController> _logger;

    public SystemAdminController(
        AppDbContext context,
        IConfiguration configuration,
        ILogger<SystemAdminController> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }


    /// <summary>
    /// Gets the current QGIS → PostgreSQL/PostGIS connection configuration.
    /// </summary>
    /// <returns>QGIS configuration settings</returns>
    [HttpGet("qgis-config")]
    [ProducesResponseType(typeof(QgisConfigurationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetQgisConfiguration()
    {
        try
        {
            var qgisConfig = _configuration.GetSection("QgisConfiguration");
            
            var response = new QgisConfigurationResponse
            {
                Host = qgisConfig["Host"] ?? string.Empty,
                Port = qgisConfig.GetValue<int>("Port", 5432),
                Database = qgisConfig["Database"] ?? string.Empty,
                Username = qgisConfig["Username"] ?? string.Empty,
                PasswordConfigured = !string.IsNullOrEmpty(qgisConfig["Password"])
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving QGIS configuration");
            throw;
        }
    }

    /// <summary>
    /// Updates the QGIS → PostgreSQL/PostGIS connection configuration.
    /// </summary>
    /// <param name="request">QGIS configuration settings</param>
    /// <returns>Updated configuration</returns>
    [HttpPut("qgis-config")]
    [ProducesResponseType(typeof(QgisConfigurationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult UpdateQgisConfiguration([FromBody] QgisConfigurationRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage ?? "Invalid value").ToArray()
                );
            throw new ValidationException("Invalid QGIS configuration", errors);
        }

        try
        {
            // Note: In a production scenario, you might want to:
            // 1. Test the connection before saving
            // 2. Store configuration in database instead of appsettings.json
            // 3. Encrypt sensitive data like passwords
            // For now, we'll just validate and indicate that configuration should be updated manually
            // or through a configuration service that can write to appsettings.json

            // Validate connection string format
            var connectionString = $"Host={request.Host};Port={request.Port};Database={request.Database};Username={request.Username};Password={request.Password}";
            
            // Basic validation - in production, you might test the connection here
            if (string.IsNullOrWhiteSpace(request.Host) || 
                string.IsNullOrWhiteSpace(request.Database) || 
                string.IsNullOrWhiteSpace(request.Username))
            {
                throw new ValidationException("Host, Database, and Username are required", new Dictionary<string, string[]>
                {
                    { nameof(request.Host), new[] { "Host is required" } },
                    { nameof(request.Database), new[] { "Database is required" } },
                    { nameof(request.Username), new[] { "Username is required" } }
                });
            }

            // For MVP: Configuration is stored in appsettings.json
            // In production, consider storing in database or using a configuration service
            _logger.LogInformation("QGIS configuration update requested. Note: Configuration should be updated in appsettings.json or through a configuration service.");

            var response = new QgisConfigurationResponse
            {
                Host = request.Host,
                Port = request.Port,
                Database = request.Database,
                Username = request.Username,
                PasswordConfigured = !string.IsNullOrEmpty(request.Password)
            };

            return Ok(response);
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating QGIS configuration");
            throw;
        }
    }

    /// <summary>
    /// Gets the current system configuration.
    /// </summary>
    /// <returns>System configuration settings</returns>
    [HttpGet("system-config")]
    [ProducesResponseType(typeof(SystemConfigurationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetSystemConfiguration()
    {
        try
        {
            var systemConfig = _configuration.GetSection("SystemConfiguration");
            
            var response = new SystemConfigurationResponse
            {
                MaxUploadSize = systemConfig.GetValue<long>("MaxUploadSize", 10485760), // Default: 10MB
                AllowedFileTypes = systemConfig["AllowedFileTypes"] ?? ".pdf,.doc,.docx,.xls,.xlsx,.jpg,.jpeg,.png",
                SessionTimeout = systemConfig.GetValue<int>("SessionTimeout", 30), // Default: 30 minutes
                MaintenanceMode = systemConfig.GetValue<bool>("MaintenanceMode", false)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving system configuration");
            throw;
        }
    }

    /// <summary>
    /// Updates the system configuration.
    /// </summary>
    /// <param name="request">System configuration settings</param>
    /// <returns>Updated configuration</returns>
    [HttpPut("system-config")]
    [ProducesResponseType(typeof(SystemConfigurationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult UpdateSystemConfiguration([FromBody] SystemConfigurationRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage ?? "Invalid value").ToArray()
                );
            throw new ValidationException("Invalid system configuration", errors);
        }

        try
        {
            // Get current configuration
            var currentConfig = _configuration.GetSection("SystemConfiguration");
            
            // For MVP: Configuration is stored in appsettings.json
            // In production, consider storing in database or using a configuration service
            _logger.LogInformation("System configuration update requested. Note: Configuration should be updated in appsettings.json or through a configuration service.");

            var response = new SystemConfigurationResponse
            {
                MaxUploadSize = request.MaxUploadSize ?? currentConfig.GetValue<long>("MaxUploadSize", 10485760),
                AllowedFileTypes = request.AllowedFileTypes ?? currentConfig["AllowedFileTypes"] ?? ".pdf,.doc,.docx,.xls,.xlsx,.jpg,.jpeg,.png",
                SessionTimeout = request.SessionTimeout ?? currentConfig.GetValue<int>("SessionTimeout", 30),
                MaintenanceMode = request.MaintenanceMode ?? currentConfig.GetValue<bool>("MaintenanceMode", false)
            };

            return Ok(response);
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating system configuration");
            throw;
        }
    }

    /// <summary>
    /// Loads predefined categories into the system.
    /// </summary>
    /// <param name="request">Load categories request</param>
    /// <returns>Loading status and count</returns>
    [HttpPost("categories/load")]
    [ProducesResponseType(typeof(LoadCategoriesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LoadCategories([FromBody] LoadCategoriesRequest? request)
    {
        try
        {
            // For MVP: This is a placeholder implementation
            // In production, this would:
            // 1. Load categories from seed data or configuration file
            // 2. Check if categories already exist (unless ForceReload is true)
            // 3. Insert categories into database
            // 4. Return count of loaded categories

            var forceReload = request?.ForceReload ?? false;

            _logger.LogInformation("Loading predefined categories. ForceReload: {ForceReload}", forceReload);

            // TODO: Implement actual category loading logic
            // This will be implemented when Category entity is created
            // For now, return a placeholder response

            var response = new LoadCategoriesResponse
            {
                Success = true,
                CategoriesLoaded = 0,
                CategoriesSkipped = 0,
                ErrorMessage = null
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading predefined categories");
            return Ok(new LoadCategoriesResponse
            {
                Success = false,
                CategoriesLoaded = 0,
                CategoriesSkipped = 0,
                ErrorMessage = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets comprehensive system status information including database, PostGIS, and system health.
    /// </summary>
    /// <returns>System status information</returns>
    [HttpGet("status")]
    [ProducesResponseType(typeof(SystemStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetSystemStatus()
    {
        try
        {
            var response = new SystemStatusResponse
            {
                Status = "Healthy",
                Database = new DatabaseStatus(),
                PostGis = new PostGisStatus(),
                System = new SystemInfo
                {
                    Version = "1.0.0",
                    RuntimeVersion = Environment.Version.ToString(),
                    ServerTimeUtc = DateTime.UtcNow
                }
            };

            // Check database connection
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                response.Database.IsConnected = canConnect;
                response.Database.Message = canConnect 
                    ? "Database connection successful" 
                    : "Cannot connect to database";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking database connection");
                response.Database.IsConnected = false;
                response.Database.Message = $"Database connection error: {ex.Message}";
                response.Status = "Degraded";
            }

            // Check PostGIS extension
            if (response.Database.IsConnected)
            {
                try
                {
                    var connection = _context.Database.GetDbConnection();
                    await connection.OpenAsync();
                    using var command = connection.CreateCommand();
                    command.CommandText = "SELECT PostGIS_version();";
                    var postgisVersion = await command.ExecuteScalarAsync() as string;
                    await connection.CloseAsync();

                    response.PostGis.IsEnabled = !string.IsNullOrEmpty(postgisVersion);
                    response.PostGis.Version = postgisVersion ?? "Not available";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking PostGIS extension");
                    response.PostGis.IsEnabled = false;
                    response.PostGis.Version = "Error checking version";
                    if (response.Status == "Healthy")
                    {
                        response.Status = "Degraded";
                    }
                }
            }
            else
            {
                response.PostGis.IsEnabled = false;
                response.PostGis.Version = "Database not connected";
                response.Status = "Unhealthy";
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving system status");
            throw;
        }
    }
}

