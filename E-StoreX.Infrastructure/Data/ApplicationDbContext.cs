using Domain.Entities.Common;
using Domain.Entities.Product;
using EStoreX.Core.Domain.Entities.Favourites;
using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.Domain.Entities.Rating;
using EStoreX.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EStoreX.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        [DbFunction("SOUNDEX", IsBuiltIn = true)]
        public static string Soundex(string input) => throw new NotSupportedException();
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<ApiClient> ApiClients { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<CategoryBrand> CategoryBrands { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
    }
}
