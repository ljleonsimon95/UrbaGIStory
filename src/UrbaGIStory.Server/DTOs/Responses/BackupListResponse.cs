namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for listing available backups.
/// </summary>
public class BackupListResponse
{
    /// <summary>
    /// List of available backups.
    /// </summary>
    public List<BackupInfo> Backups { get; set; } = new();

    /// <summary>
    /// Total number of backups.
    /// </summary>
    public int TotalCount { get; set; }
}

/// <summary>
/// Information about a single backup file.
/// </summary>
public class BackupInfo
{
    /// <summary>
    /// Unique identifier for the backup (filename without extension).
    /// </summary>
    public string BackupId { get; set; } = string.Empty;

    /// <summary>
    /// Filename of the backup file.
    /// </summary>
    public string Filename { get; set; } = string.Empty;

    /// <summary>
    /// Size of the backup file in bytes.
    /// </summary>
    public long SizeBytes { get; set; }

    /// <summary>
    /// Human-readable file size (e.g., "1.5 MB").
    /// </summary>
    public string SizeFormatted { get; set; } = string.Empty;

    /// <summary>
    /// Creation date and time of the backup.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

