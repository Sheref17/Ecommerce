using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roles =
            [
                "Admin",
                "Customer"
            ];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@ecommerce.com";
            const string adminPassword = "Admin@12345";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    FirstName = "System",
                    LastName = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail
                };

                var result = await userManager.CreateAsync(admin,adminPassword);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ",
                        result.Errors.Select(x => x.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var result = await userManager.AddToRoleAsync(admin,"Admin");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to assign Admin role.");
                }
            }
        }
    }
}
