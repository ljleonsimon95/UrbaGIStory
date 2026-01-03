using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for updating an existing entity.
/// </summary>
public class UpdateEntityRequest
{
    /// <summary>
    /// Type of entity (building, street, plaza, etc.).
    /// </summary>
    [Required(ErrorMessage = "EntityType is required")]
    public EntityType EntityType { get; set; }

    /// <summary>
    /// Optional link to QGIS geometry ID.
    /// If null, entity has no spatial representation.
    /// </summary>
    public Guid? QGISGeometryId { get; set; }

    /// <summary>
    /// Optional dynamic properties as JSON.
    /// Properties are defined by categories assigned to the entity type.
    /// </summary>
    public JsonDocument? DynamicProperties { get; set; }

    /// <summary>
    /// Row version for optimistic concurrency control.
    /// This should be the RowVersion value from the last read of the entity.
    /// </summary>
    public byte[]? RowVersion { get; set; }
}

