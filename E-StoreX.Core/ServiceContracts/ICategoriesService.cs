using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.DTO.Categories.Responses;

namespace EStoreX.Core.ServiceContracts
{
    public interface ICategoriesService
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a list of categories.</returns>
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the category.</param>
        /// <returns>A task that represents the asynchronous operation, containing the category if found; otherwise, null.</returns>
        Task<CategoryResponse?> GetCategoryByIdAsync(Guid id);
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDto">The category data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest categoryRequest);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="updateCategoryDto">new category information</param>
        /// <returns>new category</returns>
        Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDto);

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>true if success delete or false if not exist</returns>
        Task<bool> DeleteCategoryAsync(Guid id);

    }
}
