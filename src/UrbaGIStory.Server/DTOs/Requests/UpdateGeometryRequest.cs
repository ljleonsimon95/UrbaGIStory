using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for updating geometry metadata (Name, Description).
/// Note: The actual geometry (coordinates) can only be modified via QGIS.
/// </summary>
public class UpdateGeometryRequest
{
    /// <summary>
    /// New name for the geometry.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// New description for the geometry (optional).
    /// </summary>
    public string? Description { get; set; }
}

