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
        /// <inheritdoc/>
        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .FirstOrDefaultAsync(d => d.Code == code);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync()
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .Where(d =>
                    d.StartDate <= DateTime.UtcNow &&
                    (!d.EndDate.HasValue || d.EndDate.Value >= DateTime.UtcNow) &&
                    d.CurrentUsageCount < d.MaxUsageCount)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Discount>> GetExpiredDiscountsAsync()
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .Where(d =>
                    (d.EndDate.HasValue && d.EndDate.Value < DateTime.UtcNow) ||
                    d.CurrentUsageCount >= d.MaxUsageCount)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Discount>> GetNotStartedDiscountsAsync()
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .Where(d => d.StartDate > DateTime.UtcNow)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Discount?> GetActiveDiscountByCodeAsync(string code)
        {
            return await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .Include(d => d.Brand)
                .FirstOrDefaultAsync(d =>
                    d.Code == code &&
                    d.StartDate <= DateTime.UtcNow &&
                    (!d.EndDate.HasValue || d.EndDate.Value >= DateTime.UtcNow) &&
                    d.CurrentUsageCount < d.MaxUsageCount
                );
        }

    }
}
