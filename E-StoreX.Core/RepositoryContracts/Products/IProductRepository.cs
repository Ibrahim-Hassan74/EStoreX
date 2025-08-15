using Domain.Entities.Product;
using EStoreX.Core.DTO.Products.Responses;
using Microsoft.AspNetCore.Http;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Products
{
    /// <summary>
    /// Interface for managing Product-related data operations.
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        /// <summary>
        /// Adds a new product along with its associated image files.
        /// </summary>
        /// <param name="product">The product entity to be added.</param>
        /// <param name="formFiles">Collection of uploaded images for the product.</param>
        /// <returns>The created <see cref="Product"/> object with generated ID and associated photos.</returns>
        Task<Product> AddProductAsync(Product product, IFormFileCollection formFiles);

        /// <summary>
        /// Updates an existing product and replaces its associated image files.
        /// </summary>
        /// <param name="product">The updated product entity.</param>
        /// <param name="formFiles">Collection of new uploaded images to associate with the product.</param>
        /// <returns>The updated <see cref="Product"/> object.</returns>
        Task<Product> UpdateProductAsync(Product product, IFormFileCollection formFiles);

        /// <summary>
        /// Deletes the specified product from the data store.
        /// </summary>
        /// <param name="product">The product to be deleted.</param>
        /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(Product product);

        /// <summary>
        /// Retrieves a filtered, sorted, and paginated list of products based on the specified query parameters.
        /// </summary>
        /// <param name="query">Filtering, sorting, and pagination parameters.</param>
        /// <returns>A collection of <see cref="Product"/> entities that match the query.</returns>
        Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductQueryDTO query);
    }
}

