using NetTopologySuite.Geometries;

namespace UrbaGIStory.Server.Models;

/// <summary>
/// Represents a point geometry that can be edited in QGIS.
/// This table is designed to be a QGIS layer for point features.
/// </summary>
public class GeoPoint
{
    /// <summary>
    /// Unique identifier for the geometry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the geometry (visible in QGIS).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of the geometry.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The point geometry (PostGIS POINT type).
    /// SRID 4326 = WGS84 (standard GPS coordinates).
    /// </summary>
    public Point? Geometry { get; set; }

    /// <summary>
    /// Date and time when the geometry was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the geometry was last updated (UTC).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Whether the geometry is deleted (soft delete).
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Date and time when the geometry was deleted (null if not deleted).
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    // Navigation property - entities linked to this geometry
    /// <summary>
    /// Entities that are linked to this point geometry.
    /// </summary>
    public ICollection<Entity> Entities { get; set; } = new List<Entity>();
}

