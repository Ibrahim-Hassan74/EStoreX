using Domain.Entities.Product;
using EStoreX.Core.DTO.Products.Responses;

namespace EStoreX.Core.RepositoryContracts
{
    public interface IPhotoRepository : IGenericRepository<Photo>
    {
        Task<IEnumerable<Photo>> AddRangeAsync(PhotosDTO photosDTO);
        Task<IEnumerable<Photo>> UpdatePhotosAsync(PhotosDTO photosDTO);
    }
}
