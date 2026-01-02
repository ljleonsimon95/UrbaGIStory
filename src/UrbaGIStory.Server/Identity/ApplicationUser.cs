using Microsoft.AspNetCore.Identity;

namespace UrbaGIStory.Server.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    // Add custom properties here if needed in the future
    // For MVP, base IdentityUser properties are sufficient
}

