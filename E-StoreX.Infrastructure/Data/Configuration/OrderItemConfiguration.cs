using EStoreX.Core.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStoreX.Infrastructure.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            //builder.Property(x => x.Quantity)
            //    .IsRequired();
            //builder.Property(x => x.ProductItemId)
            //    .IsRequired();
            //builder.Property(x => x.MainImage)
            //    .IsRequired()
            //    .HasMaxLength(255);
            //builder.Property(x => x.ProductName)
            //    .IsRequired()
            //    .HasMaxLength(100);
        }
    }
}
