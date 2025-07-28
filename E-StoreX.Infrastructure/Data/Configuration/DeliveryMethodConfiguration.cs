using EStoreX.Core.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStoreX.Infrastructure.Data.Configuration
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasData(
                new DeliveryMethod
                {
                    Id = Guid.Parse("f5c2a7b1-4e0e-4a66-8c7a-7c9d50f9e111"),
                    Name = "Fast",
                    Description = "Fast delivery within 1-2 days",
                    DeliveryTime = "1-2 Days",
                    Price = 50
                },
                new DeliveryMethod
                {
                    Id = Guid.Parse("d9372a1e-e6cb-4d1a-9476-1f52f9c8c222"),
                    Name = "Standard",
                    Description = "Standard delivery within 3-5 days",
                    DeliveryTime = "3-5 Days",
                    Price = 20
                },
                new DeliveryMethod
                {
                    Id = Guid.Parse("6f2d385c-9b0b-4f93-aaf6-3c62d6c1d333"),
                    Name = "Economy",
                    Description = "Economy delivery within 6-8 days",
                    DeliveryTime = "6-8 Days",
                    Price = 10
                }
            );
        }
    }
}
