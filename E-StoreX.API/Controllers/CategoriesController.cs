using EStoreX.Core.DTO;
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
        public CategoriesController(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>return </returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
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

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(Id);

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
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
                return BadRequest("Category cannot be null");
            var category = categoryDTO.ToCategory();
            await _unitOfWork.CategoryRepository.AddAsync(category);
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

            if (categoryDTO is null)
                return BadRequest("Category cannot be null");

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(Id);

            if (category is null)
                return NotFound();

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;

            await _unitOfWork.CategoryRepository.UpdateAsync(category);
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
            if (Id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(Id);
            if (category is null)
                return NotFound();

            var result = await _unitOfWork.CategoryRepository.DeleteAsync(Id);
            if (!result)
                return BadRequest("Failed to delete category");

            return NoContent();

        }
    }
}
