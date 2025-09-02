using Asp.Versioning;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Enums;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Common;
using EStoreX.Core.ServiceContracts.Products;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Controller for managing brands (Admin only) in API version 2.0.
    /// </summary>
    [ApiVersion(2.0)]
    public class BrandsController : AdminControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IExportService _exportService;

        /// <summary>
        /// Initializes a new instance of <see cref="BrandsController"/>.
        /// </summary>
        /// <param name="brandService">Service to manage brand operations.</param>
        /// <param name="exportService">Service to manage files.</param>
        public BrandsController(IBrandService brandService, IExportService exportService)
        {
            _brandService = brandService;
            _exportService = exportService;
        }

        /// <summary>
        /// Creates a new brand.
        /// </summary>
        /// <param name="name">The name of the brand.</param>
        /// <returns>The created brand.</returns>
        /// <response code="200">Brand created successfully.</response>
        /// <response code="400">Brand name is empty or invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(ApiResponseFactory.NotFound("Brand name cannot be empty."));

            var brand = await _brandService.CreateBrandAsync(name);
            return Ok(brand);
        }

        /// <summary>
        /// Updates an existing brand.
        /// </summary>
        /// <param name="id">The ID of the brand to update.</param>
        /// <param name="newName">The new name for the brand.</param>
        /// <returns>The updated brand.</returns>
        /// <response code="200">Brand updated successfully.</response>
        /// <response code="400">New name is empty or invalid.</response>
        /// <response code="404">Brand not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return BadRequest(ApiResponseFactory.NotFound("New name cannot be empty."));

            var updatedBrand = await _brandService.UpdateBrandAsync(id, newName);

            if (updatedBrand == null)
                return NotFound(ApiResponseFactory.NotFound("Brand not found."));

            return Ok(updatedBrand);
        }

        /// <summary>
        /// Deletes a brand by ID.
        /// </summary>
        /// <param name="id">The ID of the brand to delete.</param>
        /// <response code="204">Brand deleted successfully.</response>
        /// <response code="404">Brand not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _brandService.DeleteBrandAsync(id);
            if (!deleted)
                return NotFound(ApiResponseFactory.NotFound("Brand not found."));

            return NoContent();
        }

        /// <summary>
        /// Exports all brands into the specified file format.
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
        /// <response code="200">brands exported successfully in the requested format.</response>
        /// <response code="400">Unsupported export type requested.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("export/{type}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Export(ExportType type)
        {
            var brands = await _brandService.GetAllBrandsAsync();

            return type switch
            {
                ExportType.Csv => File(_exportService.ExportToCsv(brands), "text/csv", "brands.csv"),
                ExportType.Excel => File(_exportService.ExportToExcel(brands), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "brands.xlsx"),
                ExportType.Pdf => File(_exportService.ExportToPdf(brands), "application/pdf", "brands.pdf"),
                _ => BadRequest(ApiResponseFactory.BadRequest("Unsupported export type"))
            };
        }
    }
}
