namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for paginated list of entities.
/// </summary>
public class EntitiesListResponse
{
    /// <summary>
    /// List of entities.
    /// </summary>
    public List<EntityResponse> Entities { get; set; } = new();

    /// <summary>
    /// Total number of entities matching the filter.
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

