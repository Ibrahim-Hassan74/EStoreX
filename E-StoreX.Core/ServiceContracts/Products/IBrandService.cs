using Domain.Entities.Product;

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
    }
}
