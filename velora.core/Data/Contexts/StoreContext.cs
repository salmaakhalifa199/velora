using Microsoft.EntityFrameworkCore;
using System.Reflection;
using velora.core.Entities;
using velora.core.Entities.IdentityEntities;
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
			modelBuilder.Entity<Feedback>(entity =>
			{
				entity.HasIndex(f => f.UserId);
			});
            modelBuilder.Entity<Order>()
           .Property(o => o.Status)
           .HasConversion<string>()
           .HasDefaultValue(OrderStatus.Placed);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderPaymentStatus)
                 .HasConversion<string>()
                .HasDefaultValue(OrderPaymentStatus.Pending);

            modelBuilder.Entity<Order>()
                .Property(o => o.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Contacts> ContactsMessages { get; set; }
        public DbSet<DeliveryMethods> DeliveryMethods { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity<Guid> baseEntity && entry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
