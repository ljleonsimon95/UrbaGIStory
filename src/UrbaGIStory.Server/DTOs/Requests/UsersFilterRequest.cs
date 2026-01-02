namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for filtering and searching users.
/// </summary>
public class UsersFilterRequest
{
    /// <summary>
    /// Filter by role name.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Filter by user status (active/inactive).
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Search term for username or email.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Page number for pagination (default: 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size for pagination (default: 20, max: 100).
    /// </summary>
    public int PageSize { get; set; } = 20;
}

