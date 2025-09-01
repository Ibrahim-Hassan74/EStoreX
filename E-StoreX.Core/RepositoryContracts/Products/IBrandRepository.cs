using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Products
{
    /// <summary>
    /// Repository contract for managing <see cref="Brand"/> entities.
    /// Provides brand-specific operations in addition to generic repository methods.
    /// </summary>
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        /// <summary>
        /// Checks whether a brand with the specified <paramref name="brandId"/> exists in the data store.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains <c>true</c> if the brand exists; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> ExistsAsync(Guid brandId);
        /// <summary>
        /// Gets a brand by its name.
        /// </summary>
        /// <param name="name">The name of the brand.</param>
        /// <returns>The <see cref="Brand"/> if found; otherwise, null.</returns>
        Task<Brand?> GetByNameAsync(string name);
        /// <summary>
        /// Retrieves all <see cref="Category"/> entities associated with the specified brand.
        /// </summary>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <returns>
        /// A collection of <see cref="Category"/> objects linked to the given brand.
        /// </returns>
        Task<IEnumerable<Category>> GetCategoriesByBrandIdAsync(Guid brandId);
    }
}
