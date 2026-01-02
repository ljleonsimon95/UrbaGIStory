namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for backup creation.
/// </summary>
public class BackupResponse
{
    /// <summary>
    /// Unique identifier for the backup.
    /// </summary>
    public string BackupId { get; set; } = string.Empty;

    /// <summary>
    /// Filename of the backup file.
    /// </summary>
    public string Filename { get; set; } = string.Empty;

    /// <summary>
    /// Full path to the backup file.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Size of the backup file in bytes.
    /// </summary>
    public long SizeBytes { get; set; }

    /// <summary>
    /// Creation date and time of the backup.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Status message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

