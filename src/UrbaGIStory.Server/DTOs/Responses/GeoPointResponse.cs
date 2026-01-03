namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for point geometry information.
/// Note: Geometry coordinates are included for visualization but cannot be modified via API.
/// </summary>
public class GeoPointResponse
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
    /// Longitude (X coordinate) of the point. Null if geometry not set.
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Latitude (Y coordinate) of the point. Null if geometry not set.
    /// </summary>
    public double? Latitude { get; set; }

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

