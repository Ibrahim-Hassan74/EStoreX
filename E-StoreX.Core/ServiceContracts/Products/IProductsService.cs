using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Products.Requests;
using EStoreX.Core.DTO.Products.Responses;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.ServiceContracts.Products
{
    public interface IProductsService
    {
        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>List of products</returns>
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product with the specified ID</returns>
        Task<ProductResponse?> GetProductByIdAsync(Guid id);
        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="productRequest">Product details to create</param>
        /// <returns>Created product</returns>
        Task<ProductResponse> CreateProductAsync(ProductAddRequest productRequest);
        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="id">Product ID to update</param>
        /// <param name="productRequest">Updated product details</param>
        /// <returns>Updated product</returns>
        Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest productUpdateRequest);
        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">Product ID to delete</param>
        Task<bool> DeleteProductAsync(Guid id);
        /// <summary>
        /// Retrieves a filtered, sorted, and paginated list of products based on the provided query parameters.
        /// </summary>
        /// <param name="query">An object containing filtering, sorting, and pagination parameters for products.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ProductResponse"/> matching the criteria.</returns>
        Task<IEnumerable<ProductResponse>> GetFilteredProductsAsync(ProductQueryDTO query);
        /// <summary>
        /// number or row in products table
        /// </summary>
        /// <returns>number of row in the products table</returns>
        Task<int> CountProductsAsync();
        /// <summary>
        /// Retrieves all images associated with the specified product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// <see cref="ApiResponse"/> containing the list of product images if found;  
        /// <c>404 Not Found</c> if the product does not exist.
        /// </returns>
        Task<ApiResponse> GetProductImagesAsync(Guid productId);

        /// <summary>
        /// Deletes a specific image from the specified product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="photoId">The unique identifier of the photo to delete.</param>
        /// <returns>
        /// <see cref="ApiResponse"/> indicating the result of the delete operation;  
        /// <c>404 Not Found</c> if the product or photo does not exist.
        /// </returns>
        Task<ApiResponse> DeleteProductImageAsync(Guid productId, Guid photoId);

        /// <summary>
        /// Adds new images to the specified product without removing existing ones.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="files">The list of image files to upload.</param>
        /// <returns>
        /// <see cref="ApiResponse"/> indicating the result of the add operation;  
        /// <c>400 Bad Request</c> if no files are provided;  
        /// <c>404 Not Found</c> if the product does not exist.
        /// </returns>
        Task<ApiResponse> AddProductImagesAsync(Guid productId, List<IFormFile> files);

        /// <summary>
        /// Replaces all existing images of the specified product with new ones.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="files">The list of new image files to upload.</param>
        /// <returns>
        /// <see cref="ApiResponse"/> indicating the result of the update operation;  
        /// <c>400 Bad Request</c> if no files are provided;  
        /// <c>404 Not Found</c> if the product does not exist.
        /// </returns>
        Task<ApiResponse> UpdateProductImagesAsync(Guid productId, List<IFormFile> files);

    }
}
