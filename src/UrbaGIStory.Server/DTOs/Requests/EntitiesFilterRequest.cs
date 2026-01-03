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
    /// Filter by point geometry ID (optional).
    /// </summary>
    public Guid? GeoPointId { get; set; }

    /// <summary>
    /// Filter by line geometry ID (optional).
    /// </summary>
    public Guid? GeoLineId { get; set; }

    /// <summary>
    /// Filter by polygon geometry ID (optional).
    /// </summary>
    public Guid? GeoPolygonId { get; set; }

    /// <summary>
    /// Filter entities that have any geometry linked (true), no geometry (false), or all (null).
    /// </summary>
    public bool? HasGeometry { get; set; }

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

