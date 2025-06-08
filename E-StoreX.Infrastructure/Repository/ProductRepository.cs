using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStoreX.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        // You can add product-specific methods here if needed
        // For example, methods to get products by category, price range, etc.
    }
}
