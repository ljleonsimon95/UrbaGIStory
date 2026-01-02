namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for paginated list of permissions.
/// </summary>
public class PermissionsListResponse
{
    /// <summary>
    /// List of permissions.
    /// </summary>
    public List<PermissionResponse> Permissions { get; set; } = new();

    /// <summary>
    /// Total number of permissions matching the filter.
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

