namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for polygon geometry information.
/// Note: Geometry coordinates are included for visualization but cannot be modified via API.
/// </summary>
public class GeoPolygonResponse
{
    /// <summary>
    /// Unique identifier for the geometry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the geometry.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of the geometry.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Array of coordinates for the exterior ring [[lon, lat], [lon, lat], ...]. Null if geometry not set.
    /// </summary>
    public double[][]? Coordinates { get; set; }

    /// <summary>
    /// Area of the polygon in square meters (calculated from geometry). Null if geometry not set.
    /// </summary>
    public double? AreaSquareMeters { get; set; }

    /// <summary>
    /// Perimeter of the polygon in meters (calculated from geometry). Null if geometry not set.
    /// </summary>
    public double? PerimeterMeters { get; set; }

    /// <summary>
    /// Date and time when the geometry was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the geometry was last updated (UTC).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Number of entities linked to this geometry.
    /// </summary>
    public int LinkedEntitiesCount { get; set; }
}

