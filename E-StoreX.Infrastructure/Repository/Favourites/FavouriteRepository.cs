using Domain.Entities.Product;
using EStoreX.Infrastructure.Data;
using EStoreX.Core.RepositoryContracts.Favourites;
using EStoreX.Core.Domain.Entities.Favourites;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Core.Repository.Favourites
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavouriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <inheritdoc/>
        public async Task<Favourite> AddToFavouriteAsync(Favourite favourite)
        {
            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync(); 
            return favourite;
        }

        /// <inheritdoc/>
        public async Task<List<Product>> GetUserFavouritesAsync(Guid userId)
        {
            return await _context.Favourites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                    .ThenInclude(p => p.Category)
                .Include(f => f.Product)
                    .ThenInclude(p => p.Photos)
                .Select(f => f.Product)
                .ToListAsync();
        }


        /// <inheritdoc/>
        public async Task<bool> IsFavouriteAsync(Favourite favourite)
        {
            return await _context.Favourites
                .AsNoTracking()
                .AnyAsync(f => f.UserId == favourite.UserId && f.ProductId == favourite.ProductId);
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFromFavouriteAsync(Favourite favourite)
        {
            var existingFavourite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.UserId == favourite.UserId && f.ProductId == favourite.ProductId);
            
            if (existingFavourite != null)
            {
                _context.Favourites.Remove(existingFavourite);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
