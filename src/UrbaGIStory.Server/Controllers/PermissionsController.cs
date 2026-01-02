using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;
using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for permission management operations.
/// All endpoints require OfficeManager role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "OfficeManager")]
public class PermissionsController : ControllerBase
{
    private readonly PermissionService _permissionService;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(
        PermissionService permissionService,
        ILogger<PermissionsController> logger)
    {
        _permissionService = permissionService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new permission assignment.
    /// </summary>
    /// <param name="request">Permission creation data</param>
    /// <returns>Created permission information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PermissionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Unauthorized();
        }

        try
        {
            var permission = await _permissionService.CreatePermissionAsync(request, currentUserId.Value);
            return CreatedAtAction(
                nameof(GetPermission),
                new { id = permission.Id },
                permission);
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Updates an existing permission assignment.
    /// </summary>
    /// <param name="id">Permission ID</param>
    /// <param name="request">Permission update data</param>
    /// <returns>Updated permission information</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PermissionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdatePermission(
        Guid id,
        [FromBody] UpdatePermissionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Unauthorized();
        }

        try
        {
            var permission = await _permissionService.UpdatePermissionAsync(id, request, currentUserId.Value);
            return Ok(permission);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = "Permission not found" });
        }
    }

    /// <summary>
    /// Deletes a permission assignment.
    /// </summary>
    /// <param name="id">Permission ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletePermission(Guid id)
    {
        try
        {
            await _permissionService.DeletePermissionAsync(id);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = "Permission not found" });
        }
    }

    /// <summary>
    /// Gets a specific permission by ID.
    /// </summary>
    /// <param name="id">Permission ID</param>
    /// <returns>Permission information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PermissionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPermission(Guid id)
    {
        try
        {
            var permission = await _permissionService.GetPermissionAsync(id);
            return Ok(permission);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = "Permission not found" });
        }
    }

    /// <summary>
    /// Gets a list of permissions with optional filtering and pagination.
    /// </summary>
    /// <param name="request">Filter and pagination options</param>
    /// <returns>Paginated list of permissions</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PermissionsListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPermissions([FromQuery] PermissionsFilterRequest request)
    {
        var response = await _permissionService.GetPermissionsAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Gets all permissions for a specific entity.
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <returns>List of permissions for the entity</returns>
    [HttpGet("entities/{entityId}")]
    [ProducesResponseType(typeof(List<PermissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetEntityPermissions(Guid entityId)
    {
        var permissions = await _permissionService.GetEntityPermissionsAsync(entityId);
        return Ok(permissions);
    }

    /// <summary>
    /// Gets the current user ID from claims.
    /// </summary>
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        return null;
    }
}

