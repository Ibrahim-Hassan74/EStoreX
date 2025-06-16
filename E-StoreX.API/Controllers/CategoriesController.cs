using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller for managing product categories in the E-StoreX application.
    /// </summary>
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
        /// <returns>return </returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Retrieve category with specific Id 
        /// </summary>
        /// <param name="Id">Category Id</param>
        /// <returns>Category with Id or BadRequest</returns>
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid Id)
        {

            var category = await _categoriesService.GetCategoryByIdAsync(Id);

            if (category is null)
                return BadRequest();

            return Ok(category);
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

            return CreatedAtAction(nameof(GetCategoryById), new { Id = category.Id }, categoryDTO);
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
