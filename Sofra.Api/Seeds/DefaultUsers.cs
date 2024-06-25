using Microsoft.AspNetCore.Identity;
using Sofra.Api.Enums;
using Sofra.Api.Models;

namespace Sofra.API.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    Email = "SuperAdmin@Sofra.com",
                    UserName = "SuberAdmin",
                    FirstName ="Abdulrhman",
                    LastName ="Elshamy",
                    Gender = Api.Enums.Gender.Male,
                    PhoneNumber="01141407630"
                };
                await userManager.CreateAsync(user,"P@ssword123");

                await userManager.AddToRolesAsync(user, new List<string>{ Roles.Admin.ToString() , Roles.Kitchen.ToString() , Roles.Customer.ToString() });
            }
        }
    }
}