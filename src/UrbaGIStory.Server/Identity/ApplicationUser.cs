using Microsoft.AspNetCore.Identity;
using UrbaGIStory.Server.Interfaces;

namespace UrbaGIStory.Server.Identity;

public class ApplicationUser : IdentityUser<Guid>, IHasConcurrency
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

    /// <summary>
    /// Row version used for optimistic concurrency control.
    /// This value is automatically updated by the database on each update.
    /// </summary>
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

