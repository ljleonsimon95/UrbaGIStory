using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for creating a permission assignment.
/// </summary>
public class CreatePermissionRequest
{
    /// <summary>
    /// ID of the user to assign the permission to.
    /// </summary>
    [Required(ErrorMessage = "UserId is required")]
    public Guid UserId { get; set; }

    /// <summary>
    /// ID of the entity this permission applies to.
    /// </summary>
    [Required(ErrorMessage = "EntityId is required")]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Whether the user can read/view the entity.
    /// </summary>
    public bool CanRead { get; set; } = true;

    /// <summary>
    /// Whether the user can write/edit the entity.
    /// </summary>
    public bool CanWrite { get; set; } = false;
}

