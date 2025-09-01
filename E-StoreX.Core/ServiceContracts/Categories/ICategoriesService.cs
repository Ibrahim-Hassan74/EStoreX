using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.DTO.Categories.Responses;

namespace EStoreX.Core.ServiceContracts.Categories
{
    /// <summary>
    /// Service contract for managing <see cref="Category"/> entities.
    /// Provides operations for creating, updating, deleting, and retrieving categories,
    /// as well as managing their associated brands.
    /// </summary>
    public interface ICategoriesService
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, 
        /// containing a collection of <see cref="CategoryResponse"/>.
        /// </returns>
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, 
        /// containing the <see cref="CategoryResponse"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<CategoryResponse?> GetCategoryByIdAsync(Guid id);

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryRequest">The category data transfer object containing input details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, 
        /// containing the newly created <see cref="CategoryResponse"/>.
        /// </returns>
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest categoryRequest);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="updateCategoryDto">The DTO containing updated category information.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, 
        /// containing the updated <see cref="CategoryResponse"/>.
        /// </returns>
        Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDto);

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>
        /// <c>true</c> if the category was successfully deleted; 
        /// otherwise, <c>false</c> if it does not exist.
        /// </returns>
        Task<bool> DeleteCategoryAsync(Guid id);

        /// <summary>
        /// Retrieves all brands assigned to a specific category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, 
        /// containing a collection of <see cref="Brand"/> entities linked to the category.
        /// </returns>
        Task<IEnumerable<Brand>> GetBrandsByCategoryIdAsync(Guid categoryId);

        /// <summary>
        /// Assigns a brand to a category.
        /// </summary>
        /// <param name="cb">The linking entity representing the relationship between category and brand.</param>
        /// <returns>
        /// <c>true</c> if the assignment was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> AssignBrandToCategoryAsync(CategoryBrand cb);

        /// <summary>
        /// Removes an existing brand assignment from a category.
        /// </summary>
        /// <param name="cb">The linking entity representing the relationship between category and brand.</param>
        /// <returns>
        /// <c>true</c> if the unassignment was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> UnassignBrandFromCategoryAsync(CategoryBrand cb);
    }
}
