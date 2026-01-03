using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;
using UrbaGIStory.Server.Helpers;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.Services;

/// <summary>
/// Service for managing urban entities.
/// </summary>
public class EntityService
{
    private readonly AppDbContext _dbContext;
    private readonly PermissionService _permissionService;
    private readonly ILogger<EntityService> _logger;

    public EntityService(
        AppDbContext dbContext,
        PermissionService permissionService,
        ILogger<EntityService> logger)
    {
        _dbContext = dbContext;
        _permissionService = permissionService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    public async Task<EntityResponse> CreateEntityAsync(
        CreateEntityRequest request,
        Guid createdBy)
    {
        var entity = new Entity
        {
            Id = Guid.NewGuid(),
            EntityType = request.EntityType,
            QGISGeometryId = request.QGISGeometryId,
            DynamicProperties = request.DynamicProperties,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            IsDeleted = false
        };

        _dbContext.Entities.Add(entity);
        await _dbContext.SaveChangesAsync();

        // Reload to get RowVersion
        await _dbContext.Entry(entity).ReloadAsync();

        _logger.LogInformation(
            "Entity created: Id: {EntityId}, Type: {EntityType}, QGISGeometryId: {QGISGeometryId}, CreatedBy: {CreatedBy}",
            entity.Id, entity.EntityType, entity.QGISGeometryId, createdBy);

        return MapToResponse(entity);
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    public async Task<EntityResponse> UpdateEntityAsync(
        Guid entityId,
        UpdateEntityRequest request,
        Guid updatedBy)
    {
        var entity = await _dbContext.Entities.FindAsync(entityId);
        if (entity == null)
        {
            throw new EntityNotFoundException("Entity", entityId);
        }

        // Check for concurrency conflict if RowVersion is provided
        if (request.RowVersion != null && !entity.RowVersion.SequenceEqual(request.RowVersion))
        {
            _logger.LogWarning(
                "Concurrency conflict detected for entity {EntityId}. Expected RowVersion: {Expected}, Actual: {Actual}",
                entityId, Convert.ToBase64String(request.RowVersion), Convert.ToBase64String(entity.RowVersion));
            throw new ConcurrencyConflictException("Entity", entityId);
        }

        // Update properties
        entity.EntityType = request.EntityType;
        entity.QGISGeometryId = request.QGISGeometryId;
        entity.DynamicProperties = request.DynamicProperties;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = updatedBy;

        // Save changes
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            await _dbContext.Entry(entity).ReloadAsync();
            _logger.LogWarning(
                "Concurrency conflict detected for entity {EntityId} during save. Current RowVersion: {RowVersion}",
                entityId, Convert.ToBase64String(entity.RowVersion));
            throw new ConcurrencyConflictException("Entity", entityId);
        }

        // Reload to get updated RowVersion
        await _dbContext.Entry(entity).ReloadAsync();

        _logger.LogInformation(
            "Entity updated: Id: {EntityId}, UpdatedBy: {UpdatedBy}",
            entityId, updatedBy);

        return MapToResponse(entity);
    }

    /// <summary>
    /// Soft deletes an entity.
    /// </summary>
    public async Task DeleteEntityAsync(Guid entityId, Guid deletedBy)
    {
        var entity = await _dbContext.Entities.FindAsync(entityId);
        if (entity == null)
        {
            throw new EntityNotFoundException("Entity", entityId);
        }

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = deletedBy;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = deletedBy;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Entity soft deleted: Id: {EntityId}, DeletedBy: {DeletedBy}", entityId, deletedBy);
    }

    /// <summary>
    /// Gets an entity by ID.
    /// </summary>
    public async Task<EntityResponse> GetEntityAsync(Guid entityId)
    {
        var entity = await _dbContext.Entities.FindAsync(entityId);
        if (entity == null)
        {
            throw new EntityNotFoundException("Entity", entityId);
        }

        return MapToResponse(entity);
    }

    /// <summary>
    /// Gets a list of entities with optional filtering and pagination.
    /// </summary>
    public async Task<EntitiesListResponse> GetEntitiesAsync(EntitiesFilterRequest request)
    {
        // Validate pagination
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1) request.PageSize = 20;
        if (request.PageSize > 100) request.PageSize = 100;

        var query = _dbContext.Entities.AsQueryable();

        // Filter by entity type
        if (request.EntityType.HasValue)
        {
            query = query.Where(e => e.EntityType == request.EntityType.Value);
        }

        // Filter by QGIS geometry ID
        if (request.QGISGeometryId.HasValue)
        {
            query = query.Where(e => e.QGISGeometryId == request.QGISGeometryId.Value);
        }

        // Text search (future: can search in dynamic properties)
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            // For now, we can search by entity type name
            // Future: search in dynamic properties JSONB
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            // This is a placeholder - actual search will be implemented when needed
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var entities = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var entityResponses = entities.Select(MapToResponse).ToList();

        return new EntitiesListResponse
        {
            Entities = entityResponses,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Checks if a user has read permission for an entity.
    /// </summary>
    public async Task<bool> CanUserReadEntityAsync(Guid userId, Guid entityId)
    {
        // Technical Administrators and Office Managers have full access
        // For now, we'll check permissions via PermissionService
        // In the future, we can add role-based checks here
        return await _permissionService.CanUserReadEntityAsync(userId, entityId);
    }

    /// <summary>
    /// Checks if a user has write permission for an entity.
    /// </summary>
    public async Task<bool> CanUserWriteEntityAsync(Guid userId, Guid entityId)
    {
        return await _permissionService.CanUserWriteEntityAsync(userId, entityId);
    }

    /// <summary>
    /// Maps Entity to EntityResponse.
    /// </summary>
    private static EntityResponse MapToResponse(Entity entity)
    {
        return new EntityResponse
        {
            Id = entity.Id,
            EntityType = entity.EntityType,
            QGISGeometryId = entity.QGISGeometryId,
            DynamicProperties = entity.DynamicProperties,
            RowVersion = entity.RowVersion,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        };
    }
}

