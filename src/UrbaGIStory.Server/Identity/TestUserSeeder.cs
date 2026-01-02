using Microsoft.AspNetCore.Identity;
using UrbaGIStory.Server.Data;

namespace UrbaGIStory.Server.Identity;

public static class TestUserSeeder
{
    public static async Task SeedTestUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // Check if test user already exists
        var testUser = await userManager.FindByNameAsync("testadmin");
        if (testUser != null)
        {
            return; // Test user already exists
        }

        // Create test user
        testUser = new ApplicationUser
        {
            UserName = "testadmin",
            Email = "testadmin@urbagistory.local",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(testUser, "TestPassword123!");
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        // Assign TechnicalAdministrator role
        var adminRole = await roleManager.FindByNameAsync("TechnicalAdministrator");
        if (adminRole != null)
        {
            await userManager.AddToRoleAsync(testUser, "TechnicalAdministrator");
        }
    }
}

