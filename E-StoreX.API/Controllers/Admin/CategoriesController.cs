using Asp.Versioning;
using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Categories;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Controller for managing product categories in the E-StoreX application.
    /// </summary>
    /// <remarks>
    /// This controller inherits from <see cref="AdminControllerBase"/> and is intended 
    /// for use in administrative scenarios where category-related operations are required.
    /// </remarks>
    [ApiVersion(2.0)]

    public class CategoriesController : AdminControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        /// <summary>
        /// Constructor for CategoriesController.
        /// </summary>
        /// <param name="categoriesService">Service to manage categories.</param>
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        /// <param name="categoryDTO">The request object containing category details.</param>
        /// <returns>
        /// Returns <see cref="CategoryResponse"/> if the category was created successfully.  
        /// Returns <see cref="BadRequestResult"/> if the request is invalid.  
        /// Returns <see cref="StatusCodeResult"/> 500 if an unexpected error occurs.
        /// </returns>
        /// <response code="200">Category created successfully.</response>
        /// <response code="400">Invalid category data supplied.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest categoryDTO)
        {
            var category = await _categoriesService.CreateCategoryAsync(categoryDTO);

            return Ok(categoryDTO);
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="Id">The unique identifier of the category to update.</param>
        /// <param name="categoryDTO">The updated category details.</param>
        /// <returns>
        /// Returns <see cref="NoContentResult"/> if the update was successful.  
        /// Returns <see cref="BadRequestResult"/> if the ID does not match the request body.  
        /// Returns <see cref="NotFoundResult"/> if no category is found with the given ID.  
        /// Returns <see cref="StatusCodeResult"/> 500 if an unexpected error occurs.
        /// </returns>
        /// <response code="204">Category updated successfully.</response>
        /// <response code="400">Invalid request or ID mismatch.</response>
        [HttpPut("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid Id, [FromBody] UpdateCategoryDTO categoryDTO)
        {
            if (Id != categoryDTO.Id)
                return BadRequest(ApiResponseFactory.BadRequest("Id mismatch"));

            var res = await _categoriesService.UpdateCategoryAsync(categoryDTO);

            return NoContent();
        }

        /// <summary>
        /// Deletes a category with the specified Id from the database.
        /// </summary>
        /// <param name="Id">The unique identifier of the category to delete.</param>
        /// <returns>
        /// Returns <see cref="NoContentResult"/> if the category was deleted successfully.  
        /// Returns <see cref="BadRequestResult"/> if deletion fails or the ID is invalid.  
        /// Returns <see cref="NotFoundResult"/> if no category exists with the given ID.  
        /// Returns <see cref="StatusCodeResult"/> 500 if an unexpected error occurs.
        /// </returns>
        /// <response code="204">Category deleted successfully.</response>
        /// <response code="400">Invalid category ID or failed to delete.</response>
        [HttpDelete("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            var result = await _categoriesService.DeleteCategoryAsync(Id);
            if (!result)
                return BadRequest(ApiResponseFactory.BadRequest("Failed to delete category"));

            return NoContent();
        }
    }
}
