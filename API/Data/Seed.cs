using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class Seed
    {
        public static async Task InitializeData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            await SeedRoles(roleManager);
            await AddAdminUser(userManager, roleManager);
        }

        public static async Task AddAdminUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var adminEmail = "admin@admin.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUser { Email = adminEmail, UserName = "admin" };

                if (await roleManager.RoleExistsAsync("Admin"))
                {
                    await userManager.CreateAsync(adminUser, "Admin66@66");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new InvalidOperationException("Admin role does not exist.");
                }
            }
        }

        public static async Task SeedRoles(RoleManager<AppRole> roleManager)
        {
            if (await roleManager.Roles.AnyAsync())
            {
                return;
            }

            var roles = new List<AppRole>
            {
                new AppRole { Name = "User" },
                new AppRole { Name = "Admin" }
            };

            foreach (var role in roles)
            {
               await roleManager.CreateAsync(role);
            }
        }
    }
}