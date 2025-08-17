using EStoreX.Core.Domain.Entities.Favourites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStoreX.Infrastructure.Data.Configuration
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.HasKey(f => new { f.UserId, f.ProductId });
            // Relationships
            builder.HasOne(f => f.User)
                   .WithMany() 
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Product)
                   .WithMany() 
                   .HasForeignKey(f => f.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
