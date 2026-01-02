namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for category loading operation.
/// </summary>
public class LoadCategoriesResponse
{
    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Number of categories loaded.
    /// </summary>
    public int CategoriesLoaded { get; set; }

    /// <summary>
    /// Number of categories skipped (already exist).
    /// </summary>
    public int CategoriesSkipped { get; set; }

    /// <summary>
    /// Error message if operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}

