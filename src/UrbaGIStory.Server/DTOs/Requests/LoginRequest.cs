using System.ComponentModel.DataAnnotations;

namespace UrbaGIStory.Server.DTOs.Requests;

/// <summary>
/// Request DTO for user login.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username or email address.
    /// </summary>
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User password.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

