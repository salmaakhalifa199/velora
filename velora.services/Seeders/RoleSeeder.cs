using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.IdentityEntities;

namespace velora.services.Seeders
{
    public static class RoleSeeder
    {

        private static readonly List<string> Roles = new() { "User", "Admin", "Guest" };

        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("RoleSeeder");

            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError($"Failed to create role '{role}': {errors}");
                    }
                    else
                    {
                        logger.LogInformation($"Role '{role}' created successfully.");
                    }
                }
            }
        }
    }
}
