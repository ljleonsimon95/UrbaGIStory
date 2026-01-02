using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for updating user information.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// New username (optional).
    /// </summary>
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string? Username { get; set; }

    /// <summary>
    /// New email address (optional).
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
    public string? Email { get; set; }

    /// <summary>
    /// New password (optional). If provided, CurrentPassword is required.
    /// </summary>
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string? Password { get; set; }

    /// <summary>
    /// Current password (required when changing password).
    /// </summary>
    public string? CurrentPassword { get; set; }
}

