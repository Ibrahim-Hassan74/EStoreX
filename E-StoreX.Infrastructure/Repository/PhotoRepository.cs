using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.Data;

namespace EStoreX.Infrastructure.Repository
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext context) : base(context)
        {
        }
        // You can add photo-specific methods here if needed
        // For example, methods to get photos by product, etc.
    }
}