using Domain.Entities.Product;
using EStoreX.Infrastructure.Data;
using EStoreX.Core.RepositoryContracts.Favourites;

namespace EStoreX.Core.Repository.Favourites
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
