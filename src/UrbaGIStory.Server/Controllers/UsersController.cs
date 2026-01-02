using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrbaGIStory.Server.DTOs.Requests;
using UrbaGIStory.Server.DTOs.Responses;
using UrbaGIStory.Server.Identity;

namespace UrbaGIStory.Server.Controllers;

/// <summary>
/// Controller for user management operations.
/// All endpoints require TechnicalAdministrator role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "TechnicalAdministrator")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<UsersController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="request">User creation data</param>
    /// <returns>Created user information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if username already exists
        var existingUser = await _userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
        {
            _logger.LogWarning("Attempt to create user with existing username: {Username}", request.Username);
            ModelState.AddModelError(nameof(request.Username), "Username already exists");
            return BadRequest(ModelState);
        }

        // Check if email already exists
        existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Attempt to create user with existing email: {Email}", request.Email);
            ModelState.AddModelError(nameof(request.Email), "Email already exists");
            return BadRequest(ModelState);
        }

        // Create new user
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true // Auto-confirm email for admin-created users
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            _logger.LogWarning("Failed to create user {Username}. Errors: {Errors}", 
                request.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest(ModelState);
        }

        // Assign default role (Specialist)
        var defaultRole = "Specialist";
        if (await _roleManager.RoleExistsAsync(defaultRole))
        {
            await _userManager.AddToRoleAsync(user, defaultRole);
            _logger.LogInformation("User {Username} assigned default role: {Role}", user.UserName, defaultRole);
        }

        _logger.LogInformation("User {Username} created successfully by administrator", user.UserName);

        var response = new UserDetailResponse
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = (await _userManager.GetRolesAsync(user)).ToList(),
            IsActive = !user.LockoutEnabled || user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
    }

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="request">User update data</param>
    /// <returns>Updated user information</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            _logger.LogWarning("Attempt to update non-existent user: {UserId}", id);
            return NotFound(new { message = "User not found" });
        }

        // Update username if provided
        if (!string.IsNullOrEmpty(request.Username) && request.Username != user.UserName)
        {
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(nameof(request.Username), "Username already exists");
                return BadRequest(ModelState);
            }
            user.UserName = request.Username;
        }

        // Update email if provided
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(nameof(request.Email), "Email already exists");
                return BadRequest(ModelState);
            }
            user.Email = request.Email;
        }

        // Update password if provided
        if (!string.IsNullOrEmpty(request.Password))
        {
            if (string.IsNullOrEmpty(request.CurrentPassword))
            {
                ModelState.AddModelError(nameof(request.CurrentPassword), "Current password is required when changing password");
                return BadRequest(ModelState);
            }

            // Change password (ChangePasswordAsync verifies current password automatically)
            var passwordResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.Password);
            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    if (error.Code == "PasswordMismatch")
                    {
                        ModelState.AddModelError(nameof(request.CurrentPassword), "Current password is incorrect");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(request.Password), error.Description);
                    }
                }
                return BadRequest(ModelState);
            }
        }

        // Save changes
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        _logger.LogInformation("User {Username} updated successfully by administrator", user.UserName);

        var response = new UserDetailResponse
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = (await _userManager.GetRolesAsync(user)).ToList(),
            IsActive = !user.LockoutEnabled || user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow,
            CreatedAt = user.Id.ToString().Length > 0 ? DateTime.UtcNow : DateTime.UtcNow // Placeholder, would need to track creation date
        };

        return Ok(response);
    }

    /// <summary>
    /// Gets a list of users with optional filtering and pagination.
    /// </summary>
    /// <param name="request">Filter and pagination options</param>
    /// <returns>Paginated list of users</returns>
    [HttpGet]
    [ProducesResponseType(typeof(UsersListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUsers([FromQuery] UsersFilterRequest request)
    {
        // Validate pagination
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1) request.PageSize = 20;
        if (request.PageSize > 100) request.PageSize = 100;

        var allUsers = _userManager.Users.AsQueryable();

        // Filter by role
        if (!string.IsNullOrEmpty(request.Role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Role);
            var userIdsInRole = usersInRole.Select(u => u.Id).ToList();
            allUsers = allUsers.Where(u => userIdsInRole.Contains(u.Id));
        }

        // Filter by active status
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
            {
                allUsers = allUsers.Where(u => !u.LockoutEnabled || u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow);
            }
            else
            {
                allUsers = allUsers.Where(u => u.LockoutEnabled && u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow);
            }
        }

        // Search by username or email
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            allUsers = allUsers.Where(u =>
                (u.UserName != null && u.UserName.ToLower().Contains(searchTerm)) ||
                (u.Email != null && u.Email.ToLower().Contains(searchTerm)));
        }

        var totalCount = allUsers.Count();

        // Apply pagination
        var users = allUsers
            .OrderBy(u => u.UserName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response
        var userDetails = new List<UserDetailResponse>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDetails.Add(new UserDetailResponse
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList(),
                IsActive = !user.LockoutEnabled || user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow,
                CreatedAt = DateTime.UtcNow // Placeholder, would need to track creation date
            });
        }

        return Ok(new UsersListResponse
        {
            Users = userDetails,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }

    /// <summary>
    /// Gets a specific user by ID.
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var roles = await _userManager.GetRolesAsync(user);

        var response = new UserDetailResponse
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = roles.ToList(),
            IsActive = !user.LockoutEnabled || user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow,
            CreatedAt = DateTime.UtcNow // Placeholder, would need to track creation date
        };

        return Ok(response);
    }
}

