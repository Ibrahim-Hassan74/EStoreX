using Domain.Entities.Product;
using EStoreX.Core.DTO.Products.Responses;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Products
{
    /// <summary>
    /// Repository interface for managing product photos.
    /// Provides methods for adding and updating photo collections.
    /// </summary>
    public interface IPhotoRepository : IGenericRepository<Photo>
    {
        /// <summary>
        /// Adds a collection of photos to a specific product.
        /// </summary>
        /// <param name="photosDTO">DTO containing the product ID and the photo details to add.</param>
        /// <returns>A collection of the added <see cref="Photo"/> entities.</returns>
        Task<IEnumerable<Photo>> AddRangeAsync(PhotosDTO photosDTO);

        /// <summary>
        /// Updates an existing collection of photos for a specific product.
        /// </summary>
        /// <param name="photosDTO">DTO containing the product ID and the updated photo details.</param>
        /// <returns>A collection of the updated <see cref="Photo"/> entities.</returns>
        Task<IEnumerable<Photo>> UpdatePhotosAsync(PhotosDTO photosDTO);
    }
}
