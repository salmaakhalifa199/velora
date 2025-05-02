using Microsoft.EntityFrameworkCore;
using System.Reflection;
using velora.core.Entities;
using velora.core.Entities.OrderEntities;


namespace velora.core.Data.Contexts
{
    public class StoreContext : DbContext
    {
        private Assembly assembly;

        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Contacts> ContactsMessages { get; set; }
        public DbSet<DeliveryMethods> DeliveryMethods { get; set; }

     


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var isBaseEntity = entry.Entity?.GetType().BaseType?.IsGenericType == true &&
                                   entry.Entity.GetType().BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>);

                if (isBaseEntity && entry.State == EntityState.Modified)
                {
                    var property = entry.Entity.GetType().GetProperty("UpdatedAt");
                    if (property != null)
                    {
                        property.SetValue(entry.Entity, DateTime.UtcNow);
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
