using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Helpers;

/// <summary>
/// Helper class for checking entity permissions.
/// This will be used by controllers when entities are implemented (Epic 5).
/// </summary>
public static class PermissionHelper
{
    /// <summary>
    /// Checks if the current user has read permission for an entity.
    /// Returns true if user has permission, false otherwise.
    /// </summary>
    public static async Task<bool> CheckReadPermissionAsync(
        PermissionService permissionService,
        Guid userId,
        Guid entityId)
    {
        return await permissionService.CanUserReadEntityAsync(userId, entityId);
    }

    /// <summary>
    /// Checks if the current user has write permission for an entity.
    /// Returns true if user has permission, false otherwise.
    /// </summary>
    public static async Task<bool> CheckWritePermissionAsync(
        PermissionService permissionService,
        Guid userId,
        Guid entityId)
    {
        return await permissionService.CanUserWriteEntityAsync(userId, entityId);
    }

    /// <summary>
    /// Throws UnauthorizedAccessException if user doesn't have read permission.
    /// </summary>
    public static async Task EnsureReadPermissionAsync(
        PermissionService permissionService,
        Guid userId,
        Guid entityId)
    {
        var hasPermission = await permissionService.CanUserReadEntityAsync(userId, entityId);
        if (!hasPermission)
        {
            throw new UnauthorizedAccessException(
                $"User does not have read permission for entity {entityId}");
        }
    }

    /// <summary>
    /// Throws UnauthorizedAccessException if user doesn't have write permission.
    /// </summary>
    public static async Task EnsureWritePermissionAsync(
        PermissionService permissionService,
        Guid userId,
        Guid entityId)
    {
        var hasPermission = await permissionService.CanUserWriteEntityAsync(userId, entityId);
        if (!hasPermission)
        {
            throw new UnauthorizedAccessException(
                $"User does not have write permission for entity {entityId}");
        }
    }
}

