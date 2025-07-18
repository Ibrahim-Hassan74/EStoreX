using EStoreX.Core.Domain.Entities;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// API controller for managing customer baskets.
    /// </summary>
    public class BasketsController : ControllerBase
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
            var basket = await _basketService.GetBasketAsync(id);
            return Ok(basket);
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
            var result = await _basketService.DeleteBasketAsync(id);
            return result ? Ok() : BadRequest();
        }
    }
}
