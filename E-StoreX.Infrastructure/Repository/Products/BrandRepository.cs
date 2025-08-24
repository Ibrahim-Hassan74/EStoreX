using Domain.Entities.Product;
using EStoreX.Core.Repository.Common;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Infrastructure.Repository.Products
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid brandId)
        {
            return await _context.Brands.AnyAsync(b => b.Id == brandId);
        }
        /// <inheritdoc/>
        public async Task<Brand?> GetByNameAsync(string name)
        {
            return await _context.Brands
                                 .FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
