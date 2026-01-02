namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for a single log entry.
/// </summary>
public class LogEntryResponse
{
    /// <summary>
    /// Timestamp of the log entry.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Log level (Information, Warning, Error, Critical).
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Log message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Exception details (if any).
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// Additional properties/context.
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();
}

