namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for system configuration.
/// </summary>
public class SystemConfigurationResponse
{
    /// <summary>
    /// Maximum file upload size in bytes.
    /// </summary>
    public long MaxUploadSize { get; set; }

    /// <summary>
    /// Allowed file types (comma-separated extensions).
    /// </summary>
    public string AllowedFileTypes { get; set; } = string.Empty;

    /// <summary>
    /// Session timeout in minutes.
    /// </summary>
    public int SessionTimeout { get; set; }

    /// <summary>
    /// Indicates if maintenance mode is enabled.
    /// </summary>
    public bool MaintenanceMode { get; set; }
}

