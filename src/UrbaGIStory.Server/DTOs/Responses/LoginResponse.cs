namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for login operation.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// JWT authentication token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration date and time (UTC).
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// Authenticated user information.
    /// </summary>
    public UserInfo User { get; set; } = new();
}

/// <summary>
/// User information included in login response.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// User unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// List of user roles.
    /// </summary>
    public List<string> Roles { get; set; } = new();
}

