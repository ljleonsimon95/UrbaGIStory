using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for updating a permission assignment.
/// </summary>
public class UpdatePermissionRequest
{
    /// <summary>
    /// Whether the user can read/view the entity.
    /// </summary>
    public bool CanRead { get; set; }

    /// <summary>
    /// Whether the user can write/edit the entity.
    /// </summary>
    public bool CanWrite { get; set; }
}

