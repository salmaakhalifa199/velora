using Microsoft.EntityFrameworkCore;
using velora.core.Entities;

namespace velora.core.Data.Configurations
{
    internal class ProductTypeConfigurations : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
