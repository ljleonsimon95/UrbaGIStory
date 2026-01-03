namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for line geometry information.
/// Note: Geometry coordinates are included for visualization but cannot be modified via API.
/// </summary>
public class GeoLineResponse
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
    /// Array of coordinates [[lon, lat], [lon, lat], ...]. Null if geometry not set.
    /// </summary>
    public double[][]? Coordinates { get; set; }

    /// <summary>
    /// Length of the line in meters (calculated from geometry). Null if geometry not set.
    /// </summary>
    public double? LengthMeters { get; set; }

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

