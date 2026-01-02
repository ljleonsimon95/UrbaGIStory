using UrbaGIStory.Server.Identity;

namespace UrbaGIStory.Server.Extensions;

/// <summary>
/// Extension methods for seeding initial data.
/// </summary>
public static class SeedConfiguration
{
    /// <summary>
    /// Seeds default roles and test user on application startup.
    /// </summary>
    public static async Task SeedInitialDataAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
            
            // Seed test user in development environment only
            if (app.Environment.IsDevelopment())
            {
                await TestUserSeeder.SeedTestUserAsync(scope.ServiceProvider);
            }
        }
    }
}

