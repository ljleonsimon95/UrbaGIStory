namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for filtering geometries list.
/// </summary>
public class GeometriesFilterRequest
{
    /// <summary>
    /// Search term for name or description (optional).
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filter geometries that have entities linked (true), no entities (false), or all (null).
    /// </summary>
    public bool? HasLinkedEntities { get; set; }

    /// <summary>
    /// Page number (default: 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size (default: 20, max: 100).
    /// </summary>
    public int PageSize { get; set; } = 20;
}

