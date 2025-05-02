using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;

namespace velora.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(orderItem => orderItem.ItemOrdered, io =>
            {
                io.Property(p => p.ProductId).HasColumnName("ProductId");
                io.Property(p => p.ProductName).HasColumnName("ProductName");
                io.Property(p => p.PictureUrl).HasColumnName("PictureUrl");
            });
        }
    }
}
