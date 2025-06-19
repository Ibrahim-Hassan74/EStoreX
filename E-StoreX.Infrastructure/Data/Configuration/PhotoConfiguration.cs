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
                ProductId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890")
            }
            );
        }
    }
}
