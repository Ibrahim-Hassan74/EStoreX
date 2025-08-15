using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Controller for managing product categories in the E-StoreX application.
    /// </summary>
    /// <remarks>This controller inherits from <see cref="AdminControllerBase"/> and is intended for use in
    /// administrative scenarios where order-related operations are required. It serves as a base for implementing
    /// actions related to order management.</remarks>
    public class CategoriesController : AdminControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        /// <summary>
        /// Constructor for CategoriesController.
        /// </summary>
        /// <param name="categoriesService"></param>
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        /// <param name="categoryDTO">categoryDTO object</param>
        /// <returns>Category created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest categoryDTO)
        {
            var category = await _categoriesService.CreateCategoryAsync(categoryDTO);

            return Ok(categoryDTO);
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="Id">category Id</param>
        /// <param name="categoryDTO">the new value for category object</param>
        /// <returns></returns>
        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid Id, [FromBody] UpdateCategoryDTO categoryDTO)
        {
            if (Id != categoryDTO.Id)
                return BadRequest("Id mismatch");

            var res = await _categoriesService.UpdateCategoryAsync(categoryDTO);

            return NoContent();
        }
        /// <summary>
        /// Deletes a category with the specified Id from the database.
        /// </summary>
        /// <param name="Id">Category Id</param>
        /// <returns></returns>
        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            var result = await _categoriesService.DeleteCategoryAsync(Id);
            if (!result)
                return BadRequest("Failed to delete category");

            return NoContent();
        }
    }
}
