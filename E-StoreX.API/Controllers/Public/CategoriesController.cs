using Asp.Versioning;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Categories;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Public
{
    /// <summary>
    /// Controller for managing product categories in the E-StoreX application.
    /// </summary>
    [ApiVersion(1.0)]
    public class CategoriesController : CustomControllerBase
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
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="CategoryResponse"/> objects representing all categories.
        /// </returns>
        /// <response code="200">
        /// OK – Returns the list of categories.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Retrieves a category by its unique ID.
        /// </summary>
        /// <param name="Id">The unique identifier (GUID) of the category to retrieve.</param>
        /// <response code="200">Successfully retrieved the category.</response>
        /// <response code="400">Invalid category ID format.</response>
        /// <response code="404">Category not found.</response>
        /// <response code="500">Unexpected error occurred.</response>
        [HttpGet("{Id:guid}")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoryById(Guid Id)
        {
            if (Id == Guid.Empty)
                return BadRequest(ApiResponseFactory.BadRequest("Invalid Category Id format"));

            var category = await _categoriesService.GetCategoryByIdAsync(Id);

            if (category is null)
                return NotFound(ApiResponseFactory.NotFound("Category not found"));

            return Ok(category);
        }

        /// <summary>
        /// Get all brands inside a category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>List of brands associated with the category.</returns>
        [HttpGet("{categoryId:guid}/brands")]
        [ProducesResponseType(typeof(IEnumerable<Brand>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrandsByCategory(Guid categoryId)
        {
            var brands = await _categoriesService.GetBrandsByCategoryIdAsync(categoryId);
            return Ok(brands);
        }
    }
}
