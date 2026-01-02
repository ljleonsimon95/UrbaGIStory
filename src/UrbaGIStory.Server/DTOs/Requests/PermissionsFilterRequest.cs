namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for filtering permissions list.
/// </summary>
public class PermissionsFilterRequest
{
    /// <summary>
    /// Filter by user ID (optional).
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Filter by entity ID (optional).
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// Page number (default: 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size (default: 20, max: 100).
    /// </summary>
    public int PageSize { get; set; } = 20;
}

