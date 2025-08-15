using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStoreX.Infrastructure.Data.Configuration
{
    public class ApiClientConfiguration : IEntityTypeConfiguration<ApiClient>
    {
        public void Configure(EntityTypeBuilder<ApiClient> builder)
        {
            builder.HasIndex(a => a.ApiKey)
                .IsUnique();
            builder.HasData(new ApiClient()
            {
                Id = Guid.Parse("125E2213-8691-45E9-AB60-D4BFA1367428"),
                ClientName = "E-StoreX flutter Client",
                ApiKey = "ovuPaA2bJcgksW6yONrlDYtKweqihHfGnd9pI1FMVRmCTzE7UBx03SXZ8QL5j4",
                IsActive = true
            });
        }
    }
}
