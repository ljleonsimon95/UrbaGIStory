namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for paginated list of users.
/// </summary>
public class UsersListResponse
{
    /// <summary>
    /// List of users.
    /// </summary>
    public List<UserDetailResponse> Users { get; set; } = new();

    /// <summary>
    /// Total number of users matching the filter criteria.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }
}

