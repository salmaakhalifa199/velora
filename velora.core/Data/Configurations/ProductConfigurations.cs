using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using velora.core.Entities;

namespace velora.core.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(p => p.Concern)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PictureUrl)
                .IsRequired();

            builder.Property(p => p.ProductCategoryId)
                .IsRequired();

            builder.HasOne(p => p.ProductCategory)
                .WithMany()
                .HasForeignKey(p => p.ProductCategoryId);

            builder.Property(p => p.ProductBrandId)
                .IsRequired();

            builder.HasOne(p => p.ProductBrand)
                .WithMany()
                .HasForeignKey(p => p.ProductBrandId);

            builder.Property(p => p.SkinType)
                .IsRequired();

        }
    }
}
