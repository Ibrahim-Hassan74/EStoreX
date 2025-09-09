using Domain.Entities.Product;
using EStoreX.Infrastructure.Repository.Common;
using EStoreX.Core.RepositoryContracts.Categories;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Infrastructure.Repository.Categories
{
    public class CategoryRepository : GenericRepository<Category> ,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Brand>> GetBrandsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.CategoryBrands
                .Where(cb => cb.CategoryId == categoryId)
                .Select(cb => cb.Brand)
                .ToListAsync();
        }

        public async Task<bool> AssignBrandAsync(CategoryBrand cb)
        {
            await _context.CategoryBrands.AddAsync(cb);
            return true;
        }

        public async Task<bool> UnassignBrandAsync(CategoryBrand cb)
        {
            var entity = await _context.CategoryBrands
                .FirstOrDefaultAsync(cb => cb.CategoryId == cb.CategoryId && cb.BrandId == cb.BrandId);

            if (entity != null)
            {
                _context.CategoryBrands.Remove(entity);
                return true;
            }
            return false;
        }
    }
}
