using Domain.Entities.Product;
using EStoreX.Infrastructure.Data;
using EStoreX.Core.RepositoryContracts.Categories;
using EStoreX.Core.Repository.Common;

namespace EStoreX.Core.Repository.Categories
{
    public class CategoryRepository : GenericRepository<Category> ,ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
