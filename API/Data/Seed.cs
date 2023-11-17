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
        public static async Task InitializeData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, DataContext context)
        {
            await SeedRoles(roleManager);
            await AddAdminUser(userManager, roleManager);
            await SeedCategories(context);
        }

        public static async Task AddAdminUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var adminEmail = "admin@example.com";
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

        public static async Task SeedCategories(DataContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            var Categories = new List<Category> {
                new Category { Name = "Clothing" },
                new Category { Name = "Shoes" },   
                new Category { Name = "Electronics" },
                new Category { Name = "Pet Supplies" }
            };

            await context.Categories.AddRangeAsync(Categories);
            await context.SaveChangesAsync();
        }

        public static async Task SeedRoles(RoleManager<AppRole> roleManager)
        {
            if (await roleManager.Roles.AnyAsync())
            {
                return;
            }

            var roles = new List<AppRole>
            {
                new AppRole { Name = "Admin" },
                new AppRole { Name = "User" }
            };

            foreach (var role in roles)
            {
               await roleManager.CreateAsync(role);
            }
        }
    }
}