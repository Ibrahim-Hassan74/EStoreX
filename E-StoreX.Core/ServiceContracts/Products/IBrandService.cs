using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.DTO.Common;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.ServiceContracts.Products
{
    /// <summary>
    /// Service contract for managing <see cref="Brand"/> entities.  
    /// Defines operations for creating, retrieving, updating, and deleting brands.
    /// </summary>
    public interface IBrandService
    {
        /// <summary>
        /// Retrieves all available brands.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains a collection of <see cref="Brand"/> entities.
        /// </returns>
        Task<IEnumerable<Brand>> GetAllBrandsAsync();

        /// <summary>
        /// Retrieves a specific brand by its unique identifier.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains the <see cref="Brand"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<Brand?> GetBrandByIdAsync(Guid brandId);

        /// <summary>
        /// Creates a new brand with the specified name.
        /// </summary>
        /// <param name="name">The name of the brand to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains the newly created <see cref="Brand"/>.
        /// </returns>
        Task<Brand> CreateBrandAsync(string name);

        /// <summary>
        /// Updates the name of an existing brand.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand to update.</param>
        /// <param name="newName">The new name to assign to the brand.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains the updated <see cref="Brand"/>.
        /// </returns>
        Task<Brand> UpdateBrandAsync(Guid brandId, string newName);

        /// <summary>
        /// Deletes a brand by its unique identifier.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand to delete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains <c>true</c> if the brand was deleted; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> DeleteBrandAsync(Guid brandId);
        /// <summary>
        /// Retrieves a brand entity from the database by its name.
        /// </summary>
        /// <param name="name">The exact name of the brand to search for.</param>
        /// <returns>
        /// Returns the <see cref="Brand"/> entity if found;  
        /// otherwise, returns <c>null</c>.
        /// </returns>
        Task<Brand?> GetBrandByNameAsync(string name);
        /// <summary>
        /// Retrieves all categories associated with a specific brand.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains a collection of <see cref="CategoryResponse"/> objects linked to the brand.
        /// </returns>
        Task<IEnumerable<CategoryResponse>> GetCategoriesByBrandIdAsync(Guid brandId);
        /// <summary>
        /// Retrieves all images associated with a specific brand.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <returns>
        /// An <see cref="ApiResponse"/> containing the list of brand images if found;  
        /// otherwise, a <c>404 Not Found</c> response.
        /// </returns>
        Task<ApiResponse> GetBrandImagesAsync(Guid brandId);

        /// <summary>
        /// Deletes a specific brand image by its ID.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <param name="photoId">The unique identifier of the photo to delete.</param>
        /// <returns>
        /// An <see cref="ApiResponse"/> indicating whether the image was deleted successfully;  
        /// <c>404 Not Found</c> if the brand or image was not found.
        /// </returns>
        Task<ApiResponse> DeleteBrandImageAsync(Guid brandId, Guid photoId);

        /// <summary>
        /// Adds images to the specified brand.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <param name="files">The list of image files to upload.</param>
        /// <returns>
        /// An <see cref="ApiResponse"/> indicating whether the images were added successfully;  
        /// <c>400 Bad Request</c> if no files were provided;  
        /// <c>404 Not Found</c> if the brand does not exist.
        /// </returns>
        Task<ApiResponse> AddBrandImagesAsync(Guid brandId, List<IFormFile> files);

        /// <summary>
        /// Updates all images of the specified brand.
        /// </summary>
        /// <remarks>
        /// This will remove all existing images of the brand and replace them with the newly uploaded ones.
        /// </remarks>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <param name="files">The list of new image files to upload.</param>
        /// <returns>
        /// An <see cref="ApiResponse"/> indicating whether the images were updated successfully;  
        /// <c>400 Bad Request</c> if no files were provided;  
        /// <c>404 Not Found</c> if the brand does not exist.
        /// </returns>
        Task<ApiResponse> UpdateBrandImagesAsync(Guid brandId, List<IFormFile> files);

    }
}
