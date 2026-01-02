namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for QGIS configuration.
/// </summary>
public class QgisConfigurationResponse
{
    /// <summary>
    /// PostgreSQL server hostname or IP address.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// PostgreSQL server port number.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Database name.
    /// </summary>
    public string Database { get; set; } = string.Empty;

    /// <summary>
    /// PostgreSQL username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if password is configured (password value is never returned for security).
    /// </summary>
    public bool PasswordConfigured { get; set; }
}

