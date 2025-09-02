using Asp.Versioning;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Enums;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Categories;
using EStoreX.Core.ServiceContracts.Common;
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
        private readonly IExportService _exportService;
        /// <summary>
        /// Constructor for CategoriesController.
        /// </summary>
        /// <param name="categoriesService">Service to manage categories.</param>
        /// <param name="exportService">Service to manage files.</param>
        public CategoriesController(ICategoriesService categoriesService, IExportService exportService)
        {
            _categoriesService = categoriesService;
            _exportService = exportService;
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

        /// <summary>
        /// Assign a brand to a category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <returns>NoContent if successful, BadRequest otherwise.</returns>
        [HttpPost("{categoryId:guid}/brands/{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignBrandToCategory(Guid categoryId, Guid brandId)
        {
            var result = await _categoriesService.AssignBrandToCategoryAsync(
                new CategoryBrand { CategoryId = categoryId, BrandId = brandId });

            if (!result)
                return BadRequest(ApiResponseFactory.BadRequest("Failed to assign brand to category."));

            return NoContent();
        }

        /// <summary>
        /// Unassign a brand from a category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <param name="brandId">The unique identifier of the brand.</param>
        /// <returns>NoContent if successful, BadRequest otherwise.</returns>
        [HttpDelete("{categoryId:guid}/brands/{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UnassignBrandFromCategory(Guid categoryId, Guid brandId)
        {
            var result = await _categoriesService.UnassignBrandFromCategoryAsync(
                new CategoryBrand { CategoryId = categoryId, BrandId = brandId });

            if (!result)
                return BadRequest(ApiResponseFactory.BadRequest("Failed to unassign brand from category."));

            return NoContent();
        }

        /// <summary>
        /// Exports all categories into the specified file format.
        /// </summary>
        /// <param name="type">
        /// The type of export format.  
        /// Supported values are:  
        /// <list type="bullet">
        ///   <item><description><see cref="ExportType.Csv"/> → Comma Separated Values file (.csv)</description></item>
        ///   <item><description><see cref="ExportType.Excel"/> → Microsoft Excel file (.xlsx)</description></item>
        ///   <item><description><see cref="ExportType.Pdf"/> → Portable Document Format file (.pdf)</description></item>
        /// </list>
        /// </param>
        /// <returns>
        /// A downloadable file in the selected export format.  
        /// Returns <see cref="BadRequestResult"/> if the format is not supported.
        /// </returns>
        /// <response code="200">categories exported successfully in the requested format.</response>
        /// <response code="400">Unsupported export type requested.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("export/{type}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Export(ExportType type)
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();

            return type switch
            {
                ExportType.Csv => File(_exportService.ExportToCsv(categories), "text/csv", "categories.csv"),
                ExportType.Excel => File(_exportService.ExportToExcel(categories), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "categories.xlsx"),
                ExportType.Pdf => File(_exportService.ExportToPdf(categories), "application/pdf", "categories.pdf"),
                _ => BadRequest(ApiResponseFactory.BadRequest("Unsupported export type"))
            };
        }
    }
}
