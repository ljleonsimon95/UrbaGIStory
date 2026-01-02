namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for loading predefined categories.
/// </summary>
public class LoadCategoriesRequest
{
    /// <summary>
    /// Force reload even if categories already exist.
    /// </summary>
    public bool ForceReload { get; set; } = false;
}

