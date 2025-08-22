using Asp.Versioning;
using EStoreX.Core.Helper;
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

        /// <summary>
        /// Initializes a new instance of <see cref="BrandsController"/>.
        /// </summary>
        /// <param name="brandService">Service to manage brand operations.</param>
        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        /// <summary>
        /// Creates a new brand.
        /// </summary>
        /// <param name="name">The name of the brand.</param>
        /// <returns>The created brand.</returns>
        /// <response code="200">Brand created successfully.</response>
        /// <response code="400">Brand name is empty or invalid.</response>
        [HttpPost]
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
        [HttpPut("{id:guid}")]
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
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _brandService.DeleteBrandAsync(id);
            if (!deleted)
                return NotFound(ApiResponseFactory.NotFound("Brand not found."));

            return NoContent();
        }
    }
}
