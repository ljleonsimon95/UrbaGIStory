using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for creating a new entity.
/// </summary>
public class CreateEntityRequest
{
    /// <summary>
    /// Type of entity (building, street, plaza, etc.).
    /// </summary>
    [Required(ErrorMessage = "EntityType is required")]
    public EntityType EntityType { get; set; }

    /// <summary>
    /// Optional link to QGIS geometry ID.
    /// If null, entity is created without spatial representation.
    /// </summary>
    public Guid? QGISGeometryId { get; set; }

    /// <summary>
    /// Optional dynamic properties as JSON.
    /// Properties are defined by categories assigned to the entity type.
    /// </summary>
    public JsonDocument? DynamicProperties { get; set; }
}

