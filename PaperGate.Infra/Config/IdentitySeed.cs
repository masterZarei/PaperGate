using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PaperGate.Core.Entities;
using PaperGate.Core.Libraries.StaticValues;

namespace PaperGate.Infra.Config;

public static class IdentitySeed
{
    public static async Task SeedAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UserInfo>>();

        // 1.If Admin role doesn't exist, Create it
        if (!await roleManager.RoleExistsAsync(Roles.AdminEndUser))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.AdminEndUser));
        }

        // 2. If Admin user doesn't exist, Create it
        var adminNationalCode = "0123456789";
        var adminUser = await userManager.FindByNameAsync(adminNationalCode);

        if (adminUser == null)
        {
            adminUser = new UserInfo
            {
                UserName = adminNationalCode,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "0123456789");
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors));
            }
        }

        // 3. Assign Admin role to Admin user
        if (!await userManager.IsInRoleAsync(adminUser, Roles.AdminEndUser))
        {
            await userManager.AddToRoleAsync(adminUser, Roles.AdminEndUser);
        }
    }
}
