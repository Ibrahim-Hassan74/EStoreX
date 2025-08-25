using Asp.Versioning;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Products;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Public
{
    /// <summary>
    /// Public controller for retrieving brand information in API version 1.0.
    /// </summary>
    [ApiVersion(1.0)]
    public class BrandsController : CustomControllerBase
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
        /// Retrieves all brands.
        /// </summary>
        /// <returns>List of all brands.</returns>
        /// <response code="200">Returns the list of brands.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Brand>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        /// <summary>
        /// Retrieves a brand by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the brand to retrieve.</param>
        /// <returns>The brand details if found.</returns>
        /// <response code="200">Brand found and returned successfully.</response>
        /// <response code="404">Brand not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
                return NotFound(ApiResponseFactory.NotFound("Brand not found."));

            return Ok(brand);
        }

        /// <summary>
        /// Retrieves a brand by its name.
        /// </summary>
        /// <param name="name">The exact name of the brand to search for.</param>
        /// <returns>
        /// Returns <see cref="Brand"/> with HTTP 200 OK if the brand is found.  
        /// Returns <see cref="ApiResponse"/> with HTTP 404 Not Found if the brand does not exist.
        /// </returns>
        /// <response code="200">Brand found and returned successfully.</response>
        /// <response code="404">Brand with the specified name was not found.</response>
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBrandByName(string name)
        {
            var brand = await _brandService.GetBrandByNameAsync(name);
            if (brand == null)
                return NotFound(ApiResponseFactory.NotFound($"Brand Not found: {name}"));
            return Ok(brand);
        }

    }
}
