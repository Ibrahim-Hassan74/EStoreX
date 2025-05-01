using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SufraX.Core.Domain.Entities;
using SufraX.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SufraX.Infrastructure.DbContext
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
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);


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

            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { IngredientId = ing1, Name = "Tomato", Description = "Fresh tomato" },
                new Ingredient { IngredientId = ing2, Name = "Cheese", Description = "Mozzarella cheese" },
                new Ingredient { IngredientId = ing3, Name = "Chicken", Description = "Grilled chicken" },
                new Ingredient { IngredientId = ing4, Name = "Basil", Description = "Fresh basil leaves" },
                new Ingredient { IngredientId = ing5, Name = "Chocolate", Description = "Dark chocolate chunks" }
            );

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

            modelBuilder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = prod1, IngredientId = ing1 }, 
                new ProductIngredient { ProductId = prod1, IngredientId = ing2 }, 
                new ProductIngredient { ProductId = prod1, IngredientId = ing4 }, 

                new ProductIngredient { ProductId = prod2, IngredientId = ing3 }, 
                new ProductIngredient { ProductId = prod2, IngredientId = ing1 }, 

                new ProductIngredient { ProductId = prod3, IngredientId = ing5 }  
            );

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
