using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStoreX.Infrastructure.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShippingAddress, x => x.WithOwner());
            builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.Status)
                .HasConversion(x => x.ToString(), x => (Status)Enum.Parse(typeof(Status), x));
            builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
}
