using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using velora.repository;
using velora.core.Data.Contexts;
using velora.services.Seeders;

namespace velora.api.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await context.Database.MigrateAsync();

                    await StoreContextSeed.SeedAsync(context, loggerFactory);

                    await RoleSeeder.SeedRolesAsync(services);
                    await UserSeeder.SeedDefaultUserAsync(services);

                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<ApplySeeding>();
                    logger.LogError(ex, "Error applying seed data");
                }
            }
        }
    }
}
