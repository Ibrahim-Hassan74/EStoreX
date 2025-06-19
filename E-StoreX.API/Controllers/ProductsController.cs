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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productsService.GetAllProductsAsync();
            return Ok(products);
        }


    }
}
