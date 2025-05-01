using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EStoreX.Core.Domain.Entities;
using EStoreX.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreX.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any additional configuration here


            var cat1 = Guid.NewGuid();
            var cat2 = Guid.NewGuid();
            var cat3 = Guid.NewGuid();

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = cat1, Name = "Appetizers" },
                new Category { CategoryId = cat2, Name = "Main Courses" },
                new Category { CategoryId = cat3, Name = "Desserts" }
            );

            var ing1 = Guid.NewGuid(); 
            var ing2 = Guid.NewGuid(); 
            var ing3 = Guid.NewGuid(); 
            var ing4 = Guid.NewGuid(); 
            var ing5 = Guid.NewGuid(); 

            var prod1 = Guid.NewGuid(); 
            var prod2 = Guid.NewGuid(); 
            var prod3 = Guid.NewGuid(); 

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = prod1,
                    Name = "Margherita Pizza",
                    Description = "Classic pizza with tomato, cheese and basil",
                    Price = 11.99m,
                    Stock = 15,
                    CategoryId = cat2
                },
                new Product
                {
                    ProductId = prod2,
                    Name = "Grilled Chicken Wrap",
                    Description = "Wrap stuffed with grilled chicken and veggies",
                    Price = 8.50m,
                    Stock = 25,
                    CategoryId = cat2
                },
                new Product
                {
                    ProductId = prod3,
                    Name = "Chocolate Lava Cake",
                    Description = "Warm cake with melted chocolate center",
                    Price = 6.00m,
                    Stock = 10,
                    CategoryId = cat3
                }
            );


        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
