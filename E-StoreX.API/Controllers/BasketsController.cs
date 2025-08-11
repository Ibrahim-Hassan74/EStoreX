using E_StoreX.API.Helper;
using EStoreX.Core.Domain.Entities;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// API controller for managing customer baskets.
    /// </summary>
    public class BasketsController : CustomControllerBase
    {
        private readonly IBasketService _basketService;
        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsController"/> class.
        /// </summary>
        /// <param name="basketService">basket service</param>
        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        /// <summary>
        /// Retrieves a customer basket by ID.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>The customer basket.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasket(string id)
        {
            if (!Guid.TryParse(id, out _))
                return BadRequest(ApiResponseFactory.BadRequest("Invalid Id format"));
            var basket = await _basketService.GetBasketAsync(id);
            return Ok(basket == null ? new CustomerBasket() : basket);
        }

        /// <summary>
        /// Adds or updates a customer basket.
        /// </summary>
        /// <param name="basket">The basket to add or update.</param>
        /// <returns>The updated basket.</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateBasket([FromBody] CustomerBasket basket)
        {
            var updatedBasket = await _basketService.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        /// <summary>
        /// Deletes a customer basket by ID.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>Status of the deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            if (!Guid.TryParse(id, out _))
                return BadRequest(ApiResponseFactory.BadRequest("Invalid Id format"));

            var result = await _basketService.DeleteBasketAsync(id);
            return result
                ? Ok(new ResponseAPI(200, "Item Deleted"))
                : BadRequest(new ResponseAPI(400, "Basket not found or already deleted"));
        }
    }
}
