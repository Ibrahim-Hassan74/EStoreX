using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.Data;

namespace EStoreX.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category> ,ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
