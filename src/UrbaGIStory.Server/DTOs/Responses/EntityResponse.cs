using System.Text.Json;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for entity information.
/// </summary>
public class EntityResponse
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Type of entity (building, street, plaza, etc.).
    /// </summary>
    public EntityType EntityType { get; set; }

    /// <summary>
    /// Optional link to a point geometry.
    /// </summary>
    public Guid? GeoPointId { get; set; }

    /// <summary>
    /// Optional link to a line geometry.
    /// </summary>
    public Guid? GeoLineId { get; set; }

    /// <summary>
    /// Optional link to a polygon geometry.
    /// </summary>
    public Guid? GeoPolygonId { get; set; }

    /// <summary>
    /// Type of geometry linked to this entity (Point, Line, Polygon, or None).
    /// </summary>
    public string? GeometryType { get; set; }

    /// <summary>
    /// Name of the linked geometry (if any).
    /// </summary>
    public string? GeometryName { get; set; }

    /// <summary>
    /// Description of the linked geometry (if any).
    /// </summary>
    public string? GeometryDescription { get; set; }

    /// <summary>
    /// Dynamic properties stored as JSON.
    /// </summary>
    public JsonDocument? DynamicProperties { get; set; }

    /// <summary>
    /// Row version for optimistic concurrency control.
    /// Include this value in update requests to detect concurrent modifications.
    /// </summary>
    public byte[]? RowVersion { get; set; }

    /// <summary>
    /// Date and time when the entity was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// ID of the user who created the entity.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the entity was last updated (UTC).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// ID of the user who last updated the entity.
    /// </summary>
    public Guid? UpdatedBy { get; set; }
}

