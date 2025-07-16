using EStoreX.Core.DTO;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller for managing products in the E-StoreX application.
    /// </summary>
    public class ProductsController : CustomControllerBase
    {
        private readonly IProductsService _productsService;
        /// <summary>
        /// Constructor for ProductsController.
        /// </summary>
        /// <param name="productsService"></param>
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts([FromQuery] ProductQueryDTO query)
        {
            var products = await _productsService.GetFilteredProductsAsync(query);

            if (!products.Any())
                return NoContent();

            return Ok(products);
        }
        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="Id">Product Id</param>
        /// <returns>Product or NotFound</returns>
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(Guid Id)
        {
            var product = await _productsService.GetProductByIdAsync(Id);
            return product is not null ? Ok(product) : NotFound();
        }
        /// <summary>
        /// Create new product in database
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateProduct(ProductAddRequest productRequest)
        {
            var createdProduct = await _productsService.CreateProductAsync(productRequest);
            return CreatedAtAction(nameof(GetProductById), new { Id = createdProduct.Id }, createdProduct);
        }
        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productUpdateRequest"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct([FromRoute] Guid id, [FromForm] ProductUpdateRequest productUpdateRequest)
        {
            if (id != productUpdateRequest.Id) return BadRequest("Id must be equals");

            var updatedProduct = await _productsService.UpdateProductAsync(productUpdateRequest);
            return Ok(updatedProduct);
        }
        /// <summary>
        /// delete product from database
        /// </summary>
        /// <param name="id">product Id</param>
        /// <returns>ok / NotFound</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty) 
                return BadRequest("Invalid Product ID");

            var res = await _productsService.DeleteProductAsync(id);

            if (!res)
                return NotFound("Can't find any product with this ID");

            return Ok("Deleted Successfully");
        }
    }
}
