namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for user detail information.
/// </summary>
public class UserDetailResponse
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

    /// <summary>
    /// Whether the user is active (not locked out).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date and time when the user was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

