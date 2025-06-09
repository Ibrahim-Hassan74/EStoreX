using EStoreX.Core.RepositoryContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller for managing product categories in the E-StoreX application.
    /// </summary>
    public class CategoriesController : CustomControllerBase
    {
        /// <summary>
        /// Constructor for CategoriesController.
        /// </summary>
        /// <param name="unitOfWork"></param>
        public CategoriesController(IUnitOfWork unitOfWork) : base(unitOfWork) {  }
        /// <summary>
        /// Retrieves all product categories from the database.
        /// </summary>
        /// <returns>return </returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return Ok(categories);
        }

    }
}
