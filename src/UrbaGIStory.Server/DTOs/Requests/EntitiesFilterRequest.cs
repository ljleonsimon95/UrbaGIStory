using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for filtering entities list.
/// </summary>
public class EntitiesFilterRequest
{
    /// <summary>
    /// Filter by entity type (optional).
    /// </summary>
    public EntityType? EntityType { get; set; }

    /// <summary>
    /// Filter by QGIS geometry ID (optional).
    /// </summary>
    public Guid? QGISGeometryId { get; set; }

    /// <summary>
    /// Search term for text search (optional).
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Page number (default: 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size (default: 20, max: 100).
    /// </summary>
    public int PageSize { get; set; } = 20;
}

