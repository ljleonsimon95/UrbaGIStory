using System.Text.Json;
using UrbaGIStory.Server.Interfaces;

namespace UrbaGIStory.Server.Models;

/// <summary>
/// Represents an urban entity in the system.
/// Entities can be linked to QGIS geometries and contain dynamic properties.
/// </summary>
public class Entity : IHasConcurrency
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
    /// Optional link to QGIS geometry.
    /// Null if entity has no spatial representation.
    /// </summary>
    public Guid? QGISGeometryId { get; set; }

    /// <summary>
    /// Dynamic properties stored as JSONB.
    /// Properties are defined by categories assigned to the entity type.
    /// </summary>
    public JsonDocument? DynamicProperties { get; set; }

    /// <summary>
    /// Row version for optimistic concurrency control.
    /// </summary>
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Whether the entity is deleted (soft delete).
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Date and time when the entity was deleted (null if not deleted).
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// ID of the user who deleted the entity (null if not deleted).
    /// </summary>
    public Guid? DeletedBy { get; set; }

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

