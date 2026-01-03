using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.Services;

/// <summary>
/// Service for managing geometries (points, lines, polygons).
/// Note: Geometry creation and coordinate editing is done via QGIS only.
/// This service handles metadata updates and deletions.
/// </summary>
public class GeometryService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<GeometryService> _logger;

    public GeometryService(AppDbContext dbContext, ILogger<GeometryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    #region GeoPoints

    /// <summary>
    /// Gets a list of point geometries with optional filtering.
    /// </summary>
    public async Task<GeometriesListResponse<GeoPointResponse>> GetGeoPointsAsync(GeometriesFilterRequest request)
    {
        var query = _dbContext.GeoPoints.AsQueryable();

        // Search by name or description
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(g => g.Name.ToLower().Contains(term) ||
                                     (g.Description != null && g.Description.ToLower().Contains(term)));
        }

        // Filter by linked entities
        if (request.HasLinkedEntities.HasValue)
        {
            if (request.HasLinkedEntities.Value)
                query = query.Where(g => g.Entities.Any());
            else
                query = query.Where(g => !g.Entities.Any());
        }

        var totalCount = await query.CountAsync();

        var geometries = await query
            .Include(g => g.Entities)
            .OrderByDescending(g => g.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new GeometriesListResponse<GeoPointResponse>
        {
            Geometries = geometries.Select(MapToGeoPointResponse).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Gets a point geometry by ID.
    /// </summary>
    public async Task<GeoPointResponse> GetGeoPointAsync(Guid id)
    {
        var geometry = await _dbContext.GeoPoints
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoPoint", id);

        return MapToGeoPointResponse(geometry);
    }

    /// <summary>
    /// Updates point geometry metadata (name, description only).
    /// </summary>
    public async Task<GeoPointResponse> UpdateGeoPointAsync(Guid id, UpdateGeometryRequest request)
    {
        var geometry = await _dbContext.GeoPoints
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoPoint", id);

        geometry.Name = request.Name;
        geometry.Description = request.Description;
        geometry.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("GeoPoint {Id} metadata updated: Name={Name}", id, request.Name);

        return MapToGeoPointResponse(geometry);
    }

    /// <summary>
    /// Deletes a point geometry. Linked entities will have their GeoPointId set to NULL.
    /// </summary>
    public async Task DeleteGeoPointAsync(Guid id)
    {
        var geometry = await _dbContext.GeoPoints
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoPoint", id);

        var linkedCount = geometry.Entities.Count;

        // Soft delete
        geometry.IsDeleted = true;
        geometry.DeletedAt = DateTime.UtcNow;

        // Clear FK references in linked entities (ON DELETE SET NULL behavior)
        foreach (var entity in geometry.Entities)
        {
            entity.GeoPointId = null;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("GeoPoint {Id} deleted. {LinkedCount} entities unlinked.", id, linkedCount);
    }

    private static GeoPointResponse MapToGeoPointResponse(GeoPoint geometry)
    {
        return new GeoPointResponse
        {
            Id = geometry.Id,
            Name = geometry.Name,
            Description = geometry.Description,
            Longitude = geometry.Geometry?.X,
            Latitude = geometry.Geometry?.Y,
            CreatedAt = geometry.CreatedAt,
            UpdatedAt = geometry.UpdatedAt,
            LinkedEntitiesCount = geometry.Entities.Count
        };
    }

    #endregion

    #region GeoLines

    /// <summary>
    /// Gets a list of line geometries with optional filtering.
    /// </summary>
    public async Task<GeometriesListResponse<GeoLineResponse>> GetGeoLinesAsync(GeometriesFilterRequest request)
    {
        var query = _dbContext.GeoLines.AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(g => g.Name.ToLower().Contains(term) ||
                                     (g.Description != null && g.Description.ToLower().Contains(term)));
        }

        if (request.HasLinkedEntities.HasValue)
        {
            if (request.HasLinkedEntities.Value)
                query = query.Where(g => g.Entities.Any());
            else
                query = query.Where(g => !g.Entities.Any());
        }

        var totalCount = await query.CountAsync();

        var geometries = await query
            .Include(g => g.Entities)
            .OrderByDescending(g => g.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new GeometriesListResponse<GeoLineResponse>
        {
            Geometries = geometries.Select(MapToGeoLineResponse).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Gets a line geometry by ID.
    /// </summary>
    public async Task<GeoLineResponse> GetGeoLineAsync(Guid id)
    {
        var geometry = await _dbContext.GeoLines
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoLine", id);

        return MapToGeoLineResponse(geometry);
    }

    /// <summary>
    /// Updates line geometry metadata (name, description only).
    /// </summary>
    public async Task<GeoLineResponse> UpdateGeoLineAsync(Guid id, UpdateGeometryRequest request)
    {
        var geometry = await _dbContext.GeoLines
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoLine", id);

        geometry.Name = request.Name;
        geometry.Description = request.Description;
        geometry.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("GeoLine {Id} metadata updated: Name={Name}", id, request.Name);

        return MapToGeoLineResponse(geometry);
    }

    /// <summary>
    /// Deletes a line geometry. Linked entities will have their GeoLineId set to NULL.
    /// </summary>
    public async Task DeleteGeoLineAsync(Guid id)
    {
        var geometry = await _dbContext.GeoLines
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoLine", id);

        var linkedCount = geometry.Entities.Count;

        geometry.IsDeleted = true;
        geometry.DeletedAt = DateTime.UtcNow;

        foreach (var entity in geometry.Entities)
        {
            entity.GeoLineId = null;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("GeoLine {Id} deleted. {LinkedCount} entities unlinked.", id, linkedCount);
    }

    private static GeoLineResponse MapToGeoLineResponse(GeoLine geometry)
    {
        double[][]? coordinates = null;
        double? length = null;

        if (geometry.Geometry != null)
        {
            coordinates = geometry.Geometry.Coordinates
                .Select(c => new[] { c.X, c.Y })
                .ToArray();
            length = geometry.Geometry.Length * 111320; // Approximate meters at equator
        }

        return new GeoLineResponse
        {
            Id = geometry.Id,
            Name = geometry.Name,
            Description = geometry.Description,
            Coordinates = coordinates,
            LengthMeters = length,
            CreatedAt = geometry.CreatedAt,
            UpdatedAt = geometry.UpdatedAt,
            LinkedEntitiesCount = geometry.Entities.Count
        };
    }

    #endregion

    #region GeoPolygons

    /// <summary>
    /// Gets a list of polygon geometries with optional filtering.
    /// </summary>
    public async Task<GeometriesListResponse<GeoPolygonResponse>> GetGeoPolygonsAsync(GeometriesFilterRequest request)
    {
        var query = _dbContext.GeoPolygons.AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(g => g.Name.ToLower().Contains(term) ||
                                     (g.Description != null && g.Description.ToLower().Contains(term)));
        }

        if (request.HasLinkedEntities.HasValue)
        {
            if (request.HasLinkedEntities.Value)
                query = query.Where(g => g.Entities.Any());
            else
                query = query.Where(g => !g.Entities.Any());
        }

        var totalCount = await query.CountAsync();

        var geometries = await query
            .Include(g => g.Entities)
            .OrderByDescending(g => g.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new GeometriesListResponse<GeoPolygonResponse>
        {
            Geometries = geometries.Select(MapToGeoPolygonResponse).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Gets a polygon geometry by ID.
    /// </summary>
    public async Task<GeoPolygonResponse> GetGeoPolygonAsync(Guid id)
    {
        var geometry = await _dbContext.GeoPolygons
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoPolygon", id);

        return MapToGeoPolygonResponse(geometry);
    }

    /// <summary>
    /// Updates polygon geometry metadata (name, description only).
    /// </summary>
    public async Task<GeoPolygonResponse> UpdateGeoPolygonAsync(Guid id, UpdateGeometryRequest request)
    {
        var geometry = await _dbContext.GeoPolygons
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoPolygon", id);

        geometry.Name = request.Name;
        geometry.Description = request.Description;
        geometry.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("GeoPolygon {Id} metadata updated: Name={Name}", id, request.Name);

        return MapToGeoPolygonResponse(geometry);
    }

    /// <summary>
    /// Deletes a polygon geometry. Linked entities will have their GeoPolygonId set to NULL.
    /// </summary>
    public async Task DeleteGeoPolygonAsync(Guid id)
    {
        var geometry = await _dbContext.GeoPolygons
            .Include(g => g.Entities)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (geometry == null)
            throw new EntityNotFoundException("GeoPolygon", id);

        var linkedCount = geometry.Entities.Count;

        geometry.IsDeleted = true;
        geometry.DeletedAt = DateTime.UtcNow;

        foreach (var entity in geometry.Entities)
        {
            entity.GeoPolygonId = null;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("GeoPolygon {Id} deleted. {LinkedCount} entities unlinked.", id, linkedCount);
    }

    private static GeoPolygonResponse MapToGeoPolygonResponse(GeoPolygon geometry)
    {
        double[][]? coordinates = null;
        double? area = null;
        double? perimeter = null;

        if (geometry.Geometry != null)
        {
            coordinates = geometry.Geometry.ExteriorRing.Coordinates
                .Select(c => new[] { c.X, c.Y })
                .ToArray();
            // Approximate conversion to meters (at equator)
            area = geometry.Geometry.Area * 111320 * 111320;
            perimeter = geometry.Geometry.Length * 111320;
        }

        return new GeoPolygonResponse
        {
            Id = geometry.Id,
            Name = geometry.Name,
            Description = geometry.Description,
            Coordinates = coordinates,
            AreaSquareMeters = area,
            PerimeterMeters = perimeter,
            CreatedAt = geometry.CreatedAt,
            UpdatedAt = geometry.UpdatedAt,
            LinkedEntitiesCount = geometry.Entities.Count
        };
    }

    #endregion
}

