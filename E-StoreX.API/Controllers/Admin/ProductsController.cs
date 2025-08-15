using Microsoft.AspNetCore.Mvc;
using EStoreX.Core.DTO.Products.Requests;
using EStoreX.Core.DTO.Products.Responses;
using EStoreX.Core.ServiceContracts.Products;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// products controller for admin operations in the E-StoreX application.
    /// </summary>
    /// <remarks>This controller inherits from <see cref="AdminControllerBase"/> and is intended for use in
    /// administrative scenarios where order-related operations are required. It serves as a base for implementing
    /// actions related to order management.</remarks>
    public class ProductsController : AdminControllerBase
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
        /// Create new product in database
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateProduct(ProductAddRequest productRequest)
        {
            var createdProduct = await _productsService.CreateProductAsync(productRequest);
            return Ok(createdProduct);
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
