using Domain.Entities.Product;
using EStoreX.Core.Enums;
using EStoreX.Infrastructure.Repository.Common;
using EStoreX.Core.RepositoryContracts.Discounts;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Infrastructure.Repositories.Discounts
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync()
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .Where(d => d.Status == DiscountStatus.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Discount>> GetExpiredDiscountsAsync()
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .Where(d => d.Status == DiscountStatus.Expired)
                .ToListAsync();
        }

        public async Task<IEnumerable<Discount>> GetNotStartedDiscountsAsync()
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .Where(d => d.Status == DiscountStatus.NotStarted)
                .ToListAsync();
        }
        public async Task<Discount?> GetActiveDiscountByCodeAsync(string code)
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .FirstOrDefaultAsync(d => d.Code == code && d.Status == DiscountStatus.Active);
        }

    }
}
