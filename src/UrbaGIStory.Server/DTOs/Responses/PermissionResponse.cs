namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for permission information.
/// </summary>
public class PermissionResponse
{
    /// <summary>
    /// Unique identifier for the permission.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID of the user who has this permission.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Username of the user who has this permission.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// ID of the entity this permission applies to.
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Whether the user can read/view the entity.
    /// </summary>
    public bool CanRead { get; set; }

    /// <summary>
    /// Whether the user can write/edit the entity.
    /// </summary>
    public bool CanWrite { get; set; }

    /// <summary>
    /// Date and time when the permission was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// ID of the Office Manager who created this permission.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the permission was last updated (UTC).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// ID of the Office Manager who last updated this permission.
    /// </summary>
    public Guid? UpdatedBy { get; set; }
}

