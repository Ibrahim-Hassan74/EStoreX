using E_StoreX.API.Helper;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Products.Responses;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Products;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Public
{
    /// <summary>
    /// Handles product-related operations such as retrieving all products or fetching product details by ID.
    /// </summary>
    public class ProductsController : CustomControllerBase
    {
        private readonly IProductsService _productsService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productsService">Service for managing product operations.</param>
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        /// <summary>
        /// Retrieves all products with optional filtering and pagination.
        /// </summary>
        /// <param name="query">Query parameters for filtering, searching, and pagination.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with a paginated list of <see cref="ProductResponse"/> objects if products exist.  
        /// Returns <see cref="NoContentResult"/> (204) if no products match the query.
        /// </returns>
        /// <response code="200">Successfully retrieved the list of products.</response>
        /// <response code="204">No products found for the given query.</response>
        /// <response code="500">Unexpected error occurred.</response>
        [HttpGet]
        [ProducesResponseType(typeof(Pagination<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts([FromQuery] ProductQueryDTO query)
        {
            var products = await _productsService.GetFilteredProductsAsync(query);

            var totalCount = await _productsService.CountProductsAsync();

            if (!products.Any())
                return NoContent();
            var result = new Pagination<ProductResponse>(query.PageNumber, query.PageSize, totalCount, products);
            return Ok(result);
        }
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the product.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with the <see cref="ProductResponse"/> if found.  
        /// Returns <see cref="NotFoundObjectResult"/> if the product does not exist.
        /// </returns>
        /// <response code="200">Successfully retrieved the product.</response>
        /// <response code="404">Product not found or invalid ID.</response>
        /// <response code="500">Unexpected error occurred.</response>
        [HttpGet("{Id:guid}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductResponse>> GetProductById(Guid Id)
        {
            var product = await _productsService.GetProductByIdAsync(Id);
            return product is not null ? Ok(product) : NotFound(ApiResponseFactory.NotFound("Not Found Product or invalid product Id"));
        }
    }
}
