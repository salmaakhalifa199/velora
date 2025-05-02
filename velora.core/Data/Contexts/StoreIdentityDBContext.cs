using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using velora.core.Entities.IdentityEntities;

namespace velora.core.Data.Contexts
{
    public class StoreIdentityDBContext : IdentityDbContext<Person>
    {
        public StoreIdentityDBContext(DbContextOptions options) : base(options) { }
             protected override void OnModelCreating(ModelBuilder builder)
             {
               base.OnModelCreating(builder);

               builder.HasDefaultSchema("Identity");

               builder.Entity<Person>().ToTable("Users");
               builder.Entity<IdentityRole>().ToTable("Roles");
               builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
               builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
               builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
               builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
               builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
             }
    }
}

