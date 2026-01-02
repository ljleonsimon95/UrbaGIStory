using Microsoft.EntityFrameworkCore;
using UrbaGIStory.Server.Data;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.Services;

/// <summary>
/// Service for managing entity permissions.
/// Permissions allow Office Managers to control which users can read/write specific entities.
/// </summary>
public class PermissionService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(
        AppDbContext dbContext,
        ILogger<PermissionService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new permission assignment.
    /// </summary>
    public async Task<PermissionResponse> CreatePermissionAsync(
        CreatePermissionRequest request,
        Guid createdBy)
    {
        // Check if permission already exists
        var existingPermission = await _dbContext.Permissions
            .FirstOrDefaultAsync(p => p.UserId == request.UserId && p.EntityId == request.EntityId);

        if (existingPermission != null)
        {
            _logger.LogWarning(
                "Permission already exists for UserId: {UserId}, EntityId: {EntityId}",
                request.UserId, request.EntityId);
            throw new ValidationException("Permission already exists for this user and entity. Use update instead.");
        }

        var permission = new Permission
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            EntityId = request.EntityId,
            CanRead = request.CanRead,
            CanWrite = request.CanWrite,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        _dbContext.Permissions.Add(permission);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Permission created: UserId: {UserId}, EntityId: {EntityId}, CanRead: {CanRead}, CanWrite: {CanWrite}",
            request.UserId, request.EntityId, request.CanRead, request.CanWrite);

        return await GetPermissionResponseAsync(permission.Id);
    }

    /// <summary>
    /// Updates an existing permission assignment.
    /// </summary>
    public async Task<PermissionResponse> UpdatePermissionAsync(
        Guid permissionId,
        UpdatePermissionRequest request,
        Guid updatedBy)
    {
        var permission = await _dbContext.Permissions.FindAsync(permissionId);
        if (permission == null)
        {
            throw new EntityNotFoundException("Permission", permissionId);
        }

        permission.CanRead = request.CanRead;
        permission.CanWrite = request.CanWrite;
        permission.UpdatedAt = DateTime.UtcNow;
        permission.UpdatedBy = updatedBy;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Permission updated: PermissionId: {PermissionId}, CanRead: {CanRead}, CanWrite: {CanWrite}",
            permissionId, request.CanRead, request.CanWrite);

        return await GetPermissionResponseAsync(permission.Id);
    }

    /// <summary>
    /// Deletes a permission assignment.
    /// </summary>
    public async Task DeletePermissionAsync(Guid permissionId)
    {
        var permission = await _dbContext.Permissions.FindAsync(permissionId);
        if (permission == null)
        {
            throw new EntityNotFoundException("Permission", permissionId);
        }

        _dbContext.Permissions.Remove(permission);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Permission deleted: PermissionId: {PermissionId}", permissionId);
    }

    /// <summary>
    /// Gets a permission by ID.
    /// </summary>
    public async Task<PermissionResponse> GetPermissionAsync(Guid permissionId)
    {
        var permission = await GetPermissionResponseAsync(permissionId);
        if (permission == null)
        {
            throw new EntityNotFoundException("Permission", permissionId);
        }

        return permission;
    }

    /// <summary>
    /// Gets a list of permissions with optional filtering and pagination.
    /// </summary>
    public async Task<PermissionsListResponse> GetPermissionsAsync(PermissionsFilterRequest request)
    {
        // Validate pagination
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1) request.PageSize = 20;
        if (request.PageSize > 100) request.PageSize = 100;

        var query = _dbContext.Permissions.AsQueryable();

        // Filter by user
        if (request.UserId.HasValue)
        {
            query = query.Where(p => p.UserId == request.UserId.Value);
        }

        // Filter by entity
        if (request.EntityId.HasValue)
        {
            query = query.Where(p => p.EntityId == request.EntityId.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var permissions = await query
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var permissionResponses = permissions.Select(p => new PermissionResponse
        {
            Id = p.Id,
            UserId = p.UserId,
            Username = p.User?.UserName ?? "Unknown",
            EntityId = p.EntityId,
            CanRead = p.CanRead,
            CanWrite = p.CanWrite,
            CreatedAt = p.CreatedAt,
            CreatedBy = p.CreatedBy,
            UpdatedAt = p.UpdatedAt,
            UpdatedBy = p.UpdatedBy
        }).ToList();

        return new PermissionsListResponse
        {
            Permissions = permissionResponses,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Gets permissions for a specific entity.
    /// </summary>
    public async Task<List<PermissionResponse>> GetEntityPermissionsAsync(Guid entityId)
    {
        var permissions = await _dbContext.Permissions
            .Include(p => p.User)
            .Where(p => p.EntityId == entityId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return permissions.Select(p => new PermissionResponse
        {
            Id = p.Id,
            UserId = p.UserId,
            Username = p.User?.UserName ?? "Unknown",
            EntityId = p.EntityId,
            CanRead = p.CanRead,
            CanWrite = p.CanWrite,
            CreatedAt = p.CreatedAt,
            CreatedBy = p.CreatedBy,
            UpdatedAt = p.UpdatedAt,
            UpdatedBy = p.UpdatedBy
        }).ToList();
    }

    /// <summary>
    /// Checks if a user has read permission for an entity.
    /// </summary>
    public async Task<bool> CanUserReadEntityAsync(Guid userId, Guid entityId)
    {
        var permission = await _dbContext.Permissions
            .FirstOrDefaultAsync(p => p.UserId == userId && p.EntityId == entityId);

        return permission?.CanRead ?? false;
    }

    /// <summary>
    /// Checks if a user has write permission for an entity.
    /// </summary>
    public async Task<bool> CanUserWriteEntityAsync(Guid userId, Guid entityId)
    {
        var permission = await _dbContext.Permissions
            .FirstOrDefaultAsync(p => p.UserId == userId && p.EntityId == entityId);

        return permission?.CanWrite ?? false;
    }

    /// <summary>
    /// Gets permission response by permission ID.
    /// </summary>
    private async Task<PermissionResponse> GetPermissionResponseAsync(Guid permissionId)
    {
        var permission = await _dbContext.Permissions
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == permissionId);

        if (permission == null)
        {
            throw new EntityNotFoundException("Permission", permissionId);
        }

        return new PermissionResponse
        {
            Id = permission.Id,
            UserId = permission.UserId,
            Username = permission.User?.UserName ?? "Unknown",
            EntityId = permission.EntityId,
            CanRead = permission.CanRead,
            CanWrite = permission.CanWrite,
            CreatedAt = permission.CreatedAt,
            CreatedBy = permission.CreatedBy,
            UpdatedAt = permission.UpdatedAt,
            UpdatedBy = permission.UpdatedBy
        };
    }
}

