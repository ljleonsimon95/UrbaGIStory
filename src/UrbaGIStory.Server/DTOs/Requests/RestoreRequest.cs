using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for restoring a database backup.
/// </summary>
public class RestoreRequest
{
    /// <summary>
    /// Filename or ID of the backup to restore.
    /// </summary>
    [Required(ErrorMessage = "Backup filename or ID is required")]
    public string BackupId { get; set; } = string.Empty;

    /// <summary>
    /// Whether to create an automatic backup before restoring (safety measure).
    /// Default: true
    /// </summary>
    public bool CreateBackupBeforeRestore { get; set; } = true;
}

