using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EStoreX.Infrastructure.Data.Configuration
{
    internal class CategoryBrandConfiguration : IEntityTypeConfiguration<CategoryBrand>
    {
        public void Configure(EntityTypeBuilder<CategoryBrand> builder)
        {
            builder.HasKey(cb => new { cb.CategoryId, cb.BrandId });

            builder.HasOne(cb => cb.Category)
                .WithMany(c => c.CategoryBrands)
                .HasForeignKey(cb => cb.CategoryId);

            builder.HasOne(cb => cb.Brand)
                .WithMany(b => b.CategoryBrands)
                .HasForeignKey(cb => cb.BrandId);

        }
    }
}
