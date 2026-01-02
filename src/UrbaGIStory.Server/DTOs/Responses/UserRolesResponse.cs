namespace UrbaGIStory.Server.DTOs.Responses;

/// <summary>
/// Response DTO for user roles information.
/// </summary>
public class UserRolesResponse
{
    /// <summary>
    /// User unique identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// List of roles assigned to the user.
    /// </summary>
    public List<string> Roles { get; set; } = new();
}

