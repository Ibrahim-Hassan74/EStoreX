using Domain.Entities.Product;
using EStoreX.Core.DTO;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.RepositoryContracts
{
    public interface IPhotoRepository : IGenericRepository<Photo>
    {
        Task<IEnumerable<Photo>> AddRangeAsync(PhotosDTO photosDTO);
    }
}
