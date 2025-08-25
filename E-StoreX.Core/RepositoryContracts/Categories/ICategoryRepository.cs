using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // for Adding method only related to Category
        Task<IEnumerable<Brand>> GetBrandsByCategoryIdAsync(Guid categoryId);
        Task<bool> AssignBrandAsync(CategoryBrand cb);
        Task<bool> UnassignBrandAsync(CategoryBrand cb);
    }
}
