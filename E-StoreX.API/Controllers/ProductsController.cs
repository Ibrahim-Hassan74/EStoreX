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
        public async Task<ActionResult<IEnumerable<ProductResponse>?>> GetAllProducts()
        {
            var products = await _productsService.GetAllProductsAsync();
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
        public async Task<ActionResult<ProductResponse>> CreateProduct(ProductRequest productRequest)
        {
            var createdProduct = await _productsService.CreateProductAsync(productRequest);
            return CreatedAtAction(nameof(GetProductById), new { Id = createdProduct.Id }, createdProduct);
        }


    }
}
