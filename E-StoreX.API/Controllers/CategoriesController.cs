using EStoreX.Core.ServiceContracts;
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
    }
}
