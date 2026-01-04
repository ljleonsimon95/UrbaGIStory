namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for paginated list of geometries.
/// </summary>
/// <typeparam name="T">The geometry response type (GeoPointResponse, GeoLineResponse, or GeoPolygonResponse)</typeparam>
public class GeometriesListResponse<T>
{
    /// <summary>
    /// List of geometries.
    /// </summary>
    public List<T> Geometries { get; set; } = new();

    /// <summary>
    /// Total count of geometries matching the filter.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }
}

