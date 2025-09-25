using EStoreX.Core.Domain.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EStoreX.Infrastructure.Data.Configuration
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasAlternateKey(u => u.Email);
            builder.HasMany(u => u.Ratings)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.Buyer)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Photo)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
