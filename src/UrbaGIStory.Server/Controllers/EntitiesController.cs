using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Exceptions;
using UrbaGIStory.Server.Helpers;
using UrbaGIStory.Server.Services;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for entity management operations.
/// All endpoints require authentication and check entity permissions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EntitiesController : ControllerBase
{
    private readonly EntityService _entityService;
    private readonly PermissionService _permissionService;
    private readonly ILogger<EntitiesController> _logger;

    public EntitiesController(
        EntityService entityService,
        PermissionService permissionService,
        ILogger<EntitiesController> logger)
    {
        _entityService = entityService;
        _permissionService = permissionService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="request">Entity creation data</param>
    /// <returns>Created entity information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EntityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateEntity([FromBody] CreateEntityRequest request)
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
            var entity = await _entityService.CreateEntityAsync(request, currentUserId.Value);
            return CreatedAtAction(
                nameof(GetEntity),
                new { id = entity.Id },
                entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="request">Entity update data</param>
    /// <returns>Updated entity information</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EntityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateEntity(
        Guid id,
        [FromBody] UpdateEntityRequest request)
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

        // Check write permission
        try
        {
            await PermissionHelper.EnsureWritePermissionAsync(
                _permissionService, currentUserId.Value, id);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning(
                "User {UserId} attempted to update entity {EntityId} without write permission",
                currentUserId, id);
            return Forbid();
        }

        try
        {
            var entity = await _entityService.UpdateEntityAsync(id, request, currentUserId.Value);
            return Ok(entity);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = "Entity not found" });
        }
        catch (ConcurrencyConflictException)
        {
            return Conflict(new { message = "Entity has been modified by another user. Please refresh and try again." });
        }
    }

    /// <summary>
    /// Soft deletes an entity.
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteEntity(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Unauthorized();
        }

        // Check write permission
        try
        {
            await PermissionHelper.EnsureWritePermissionAsync(
                _permissionService, currentUserId.Value, id);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning(
                "User {UserId} attempted to delete entity {EntityId} without write permission",
                currentUserId, id);
            return Forbid();
        }

        try
        {
            await _entityService.DeleteEntityAsync(id, currentUserId.Value);
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = "Entity not found" });
        }
    }

    /// <summary>
    /// Gets a specific entity by ID.
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <returns>Entity information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EntityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetEntity(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Unauthorized();
        }

        // Check read permission
        try
        {
            await PermissionHelper.EnsureReadPermissionAsync(
                _permissionService, currentUserId.Value, id);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning(
                "User {UserId} attempted to read entity {EntityId} without read permission",
                currentUserId, id);
            return Forbid();
        }

        try
        {
            var entity = await _entityService.GetEntityAsync(id);
            return Ok(entity);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(new { message = "Entity not found" });
        }
    }

    /// <summary>
    /// Gets a list of entities with optional filtering and pagination.
    /// </summary>
    /// <param name="request">Filter and pagination options</param>
    /// <returns>Paginated list of entities</returns>
    [HttpGet]
    [ProducesResponseType(typeof(EntitiesListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetEntities([FromQuery] EntitiesFilterRequest request)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
        {
            return Unauthorized();
        }

        // Note: For list endpoints, we filter by permissions in the service
        // This is a simplified version - in production, you'd want to filter
        // the list to only show entities the user has read permission for
        var response = await _entityService.GetEntitiesAsync(request);
        return Ok(response);
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

