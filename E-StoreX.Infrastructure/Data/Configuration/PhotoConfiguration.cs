using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStoreX.Infrastructure.Data.Configuration
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasData(new Photo
            {
                Id = Guid.NewGuid(),
                ImageName = "default.jpg",
                ProductId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890")
            }
            );
        }
    }
}
