using Microsoft.AspNetCore.Identity;

namespace UrbaGIStory.Server.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    // Add custom properties here if needed in the future
    // For MVP, base IdentityRole properties are sufficient
}

