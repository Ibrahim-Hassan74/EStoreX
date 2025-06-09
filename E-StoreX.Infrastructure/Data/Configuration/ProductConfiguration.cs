﻿using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStoreX.Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(18, 2)").IsRequired();
            
            builder.HasData(new Product
            {
                Id = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890"),
                Name = "Sample Product",
                Description = "This is a sample product description.",
                Price = 19.99m,
                CategoryId = Guid.Parse("19F389FE-8472-46FC-83EA-2440790A2067")
            });
        }
    }
}
