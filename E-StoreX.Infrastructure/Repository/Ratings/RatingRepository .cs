using EStoreX.Core.Domain.Entities.Rating;
using EStoreX.Core.Repository.Common;
using EStoreX.Core.RepositoryContracts.Ratings;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Infrastructure.Repository.Ratings
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Rating>> GetRatingsByProductAsync(Guid productId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }


        /// <inheritdoc/>
        public async Task<double> GetAverageRatingAsync(Guid productId)
        {
            return await _context.Ratings
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => (double)r.Score);
        }
        /// <inheritdoc/>
        public async Task<Rating?> GetUserRatingForProductAsync(Guid productId, Guid userId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
        }
    }

}
