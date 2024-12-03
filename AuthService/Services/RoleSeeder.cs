using Microsoft.AspNetCore.Identity;

namespace AuthService.Services;

public static class RoleSeeder
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        // Get the RoleManager service from the service provider
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Define the roles to be created
        string[] roles = { "User", "Expert", "Company", "Moderator", "Admin", "SuperAdmin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
