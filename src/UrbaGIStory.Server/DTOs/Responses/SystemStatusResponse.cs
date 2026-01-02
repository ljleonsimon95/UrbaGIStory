namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for system status information.
/// </summary>
public class SystemStatusResponse
{
    /// <summary>
    /// Overall system health status.
    /// </summary>
    public string Status { get; set; } = "Unknown";

    /// <summary>
    /// Database connection status.
    /// </summary>
    public DatabaseStatus Database { get; set; } = new();

    /// <summary>
    /// PostGIS extension status.
    /// </summary>
    public PostGisStatus PostGis { get; set; } = new();

    /// <summary>
    /// System information.
    /// </summary>
    public SystemInfo System { get; set; } = new();
}

/// <summary>
/// Database connection status information.
/// </summary>
public class DatabaseStatus
{
    /// <summary>
    /// Indicates if database connection is available.
    /// </summary>
    public bool IsConnected { get; set; }

    /// <summary>
    /// Database connection message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// PostGIS extension status information.
/// </summary>
public class PostGisStatus
{
    /// <summary>
    /// Indicates if PostGIS extension is installed and enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// PostGIS version string.
    /// </summary>
    public string? Version { get; set; }
}

/// <summary>
/// System information.
/// </summary>
public class SystemInfo
{
    /// <summary>
    /// Application version.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// .NET runtime version.
    /// </summary>
    public string RuntimeVersion { get; set; } = string.Empty;

    /// <summary>
    /// Server time (UTC).
    /// </summary>
    public DateTime ServerTimeUtc { get; set; } = DateTime.UtcNow;
}

