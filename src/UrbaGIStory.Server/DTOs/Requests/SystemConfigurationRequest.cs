using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for updating system configuration.
/// </summary>
public class SystemConfigurationRequest
{
    /// <summary>
    /// Maximum file upload size in bytes.
    /// </summary>
    [Range(1024, long.MaxValue, ErrorMessage = "MaxUploadSize must be at least 1024 bytes")]
    public long? MaxUploadSize { get; set; }

    /// <summary>
    /// Allowed file types (comma-separated extensions).
    /// </summary>
    [StringLength(500, ErrorMessage = "AllowedFileTypes must not exceed 500 characters")]
    public string? AllowedFileTypes { get; set; }

    /// <summary>
    /// Session timeout in minutes.
    /// </summary>
    [Range(1, 1440, ErrorMessage = "SessionTimeout must be between 1 and 1440 minutes")]
    public int? SessionTimeout { get; set; }

    /// <summary>
    /// Enable or disable maintenance mode.
    /// </summary>
    public bool? MaintenanceMode { get; set; }
}

