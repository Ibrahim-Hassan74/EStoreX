using EStoreX.Core.DTO;

namespace EStoreX.Core.ServiceContracts
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
        Task<ProductResponse> CreateProductAsync(ProductRequest productRequest);
        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="id">Product ID to update</param>
        /// <param name="productRequest">Updated product details</param>
        /// <returns>Updated product</returns>
        Task<ProductResponse> UpdateProductAsync(Guid id, ProductRequest productRequest);
        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">Product ID to delete</param>
        Task DeleteProductAsync(Guid id);
    }
}
