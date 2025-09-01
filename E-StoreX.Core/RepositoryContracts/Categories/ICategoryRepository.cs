using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Categories
{
    /// <summary>
    /// Repository interface for handling data operations related to <see cref="Category"/>.
    /// Extends the generic repository with category-specific operations.
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        /// <summary>
        /// Retrieves all <see cref="Brand"/> entities assigned to the given category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>
        /// A collection of <see cref="Brand"/> objects that are linked to the specified category.
        /// </returns>
        Task<IEnumerable<Brand>> GetBrandsByCategoryIdAsync(Guid categoryId);

        /// <summary>
        /// Assigns a <see cref="Brand"/> to a <see cref="Category"/>.
        /// </summary>
        /// <param name="cb">The linking entity that represents the relationship between category and brand.</param>
        /// <returns>
        /// <c>true</c> if the assignment was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> AssignBrandAsync(CategoryBrand cb);

        /// <summary>
        /// Removes an existing assignment of a <see cref="Brand"/> from a <see cref="Category"/>.
        /// </summary>
        /// <param name="cb">The linking entity that represents the relationship between category and brand.</param>
        /// <returns>
        /// <c>true</c> if the unassignment was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> UnassignBrandAsync(CategoryBrand cb);
    }
}
