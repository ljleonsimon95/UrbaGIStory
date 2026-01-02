using Microsoft.AspNetCore.Identity;

namespace UrbaGIStory.Server.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Indicates whether the user is active (not deactivated).
    /// When false, the user cannot log in.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date and time when the user was deactivated (null if active).
    /// </summary>
    public DateTime? DeactivatedAt { get; set; }

    /// <summary>
    /// ID of the administrator who deactivated the user (null if active).
    /// </summary>
    public Guid? DeactivatedBy { get; set; }
}

