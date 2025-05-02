using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using velora.core.Entities.IdentityEntities;

namespace velora.services.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedDefaultUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Person>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("UserSeeder");


            string userEmail = config["Seeding:DefaultUser:Email"];
            string userPassword = config["Seeding:DefaultUser:Password"];
            await CreateUserIfNotExists(userManager, roleManager, userEmail, userPassword, "User", logger);

            string adminEmail = config["Seeding:AdminUser:Email"];
            string adminPassword = config["Seeding:AdminUser:Password"];
            await CreateUserIfNotExists(userManager, roleManager, adminEmail, adminPassword, "Admin", logger);
        }

        private static async Task CreateUserIfNotExists(UserManager<Person> userManager, RoleManager<IdentityRole> roleManager, string email, string password, string role, ILogger logger)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var newUser = new Person
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "Default",  
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    await EnsureRoleExists(roleManager, role, logger);
                    await userManager.AddToRoleAsync(newUser, role);
                    logger.LogInformation($"User {newUser.UserName} assigned to {role} role.");
                }
                else
                {
                    logger.LogError($"Failed to create user {newUser.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                var roles = await userManager.GetRolesAsync(existingUser);
                if (!roles.Contains(role))
                {
                    await EnsureRoleExists(roleManager, role, logger);
                    await userManager.AddToRoleAsync(existingUser, role);
                    logger.LogInformation($"Role {role} assigned to {existingUser.UserName}.");
                }
            }
        }

        private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string role, ILogger logger)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (roleResult.Succeeded)
                {
                    logger.LogInformation($"Role {role} created successfully.");
                }
                else
                {
                    logger.LogError($"Failed to create role {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
