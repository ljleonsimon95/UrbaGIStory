using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for assigning a role to a user.
/// </summary>
public class AssignRoleRequest
{
    /// <summary>
    /// Role name to assign. Must be one of: TechnicalAdministrator, OfficeManager, Specialist
    /// </summary>
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(50, ErrorMessage = "Role name must not exceed 50 characters")]
    public string RoleName { get; set; } = string.Empty;
}

