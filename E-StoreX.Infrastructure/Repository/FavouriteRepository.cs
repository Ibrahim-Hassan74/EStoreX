using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.Data;

namespace EStoreX.Infrastructure.Repository
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavouriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task AddToFavouriteAsync(string userId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetUserFavouritesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFavouriteAsync(string userId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromFavouriteAsync(string userId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
