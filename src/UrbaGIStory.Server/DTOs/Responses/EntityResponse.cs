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
    /// Optional link to QGIS geometry ID.
    /// </summary>
    public Guid? QGISGeometryId { get; set; }

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

