using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaperGate.Core.Entities;
using PaperGate.Core.Libraries.StaticValues;

namespace PaperGate.Infra.Config;

public static class IdentitySeed
{
    public static async Task SeedAdmin(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var adminSection = configuration.GetSection("AdminSeed");

        var adminNationalCode = adminSection["NationalCode"];
        var adminPassword = adminSection["Password"];

        if (string.IsNullOrWhiteSpace(adminNationalCode) || string.IsNullOrWhiteSpace(adminPassword))
            return;

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UserInfo>>();

        if (!await roleManager.RoleExistsAsync(Roles.AdminEndUser))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.AdminEndUser));
        }

        var adminUser = await userManager.FindByNameAsync(adminNationalCode);

        if (adminUser == null)
        {
            adminUser = new UserInfo
            {
                UserName = adminNationalCode,
                NationalCode = adminNationalCode,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors));
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, Roles.AdminEndUser))
        {
            await userManager.AddToRoleAsync(adminUser, Roles.AdminEndUser);
        }
    }
}
