using Microsoft.AspNetCore.Identity;
using Sofra.Api.Enums;

namespace Sofra.Api.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Kitchen.ToString()));
            }
        }
    }
}