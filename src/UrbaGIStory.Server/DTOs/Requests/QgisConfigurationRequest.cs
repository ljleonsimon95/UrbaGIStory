using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for updating QGIS → PostgreSQL/PostGIS connection configuration.
/// </summary>
public class QgisConfigurationRequest
{
    /// <summary>
    /// PostgreSQL server hostname or IP address.
    /// </summary>
    [Required(ErrorMessage = "Host is required")]
    [StringLength(255, ErrorMessage = "Host must not exceed 255 characters")]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// PostgreSQL server port number.
    /// </summary>
    [Required(ErrorMessage = "Port is required")]
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
    public int Port { get; set; }

    /// <summary>
    /// Database name.
    /// </summary>
    [Required(ErrorMessage = "Database name is required")]
    [StringLength(100, ErrorMessage = "Database name must not exceed 100 characters")]
    public string Database { get; set; } = string.Empty;

    /// <summary>
    /// PostgreSQL username.
    /// </summary>
    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, ErrorMessage = "Username must not exceed 100 characters")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// PostgreSQL password.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [StringLength(500, ErrorMessage = "Password must not exceed 500 characters")]
    public string Password { get; set; } = string.Empty;
}

