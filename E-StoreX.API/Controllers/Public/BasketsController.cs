using Domain.Entities.Baskets;
using Microsoft.AspNetCore.Mvc;
using EStoreX.Core.ServiceContracts.Basket;
using EStoreX.Core.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EStoreX.Core.DTO.Basket;

namespace E_StoreX.API.Controllers.Public
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
        public async Task<IActionResult> AddOrUpdateBasket([FromBody] CustomerBasketDTO basket)
        {
            if (!Guid.TryParse(basket.Id, out _))
                return BadRequest(ApiResponseFactory.BadRequest("Invalid Id format"));

            var updatedBasket = await _basketService.UpdateBasketAsync(basket);
            if (updatedBasket == null)
                return BadRequest(ApiResponseFactory.BadRequest("No valid items to update the basket"));
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
                ? Ok(ApiResponseFactory.Success("Item Deleted"))
                : BadRequest(ApiResponseFactory.NotFound("Basket not found or already deleted"));
        }
        /// <summary>
        /// Merges the guest basket (created before login) with the authenticated user's basket.
        /// </summary>
        /// <param name="guestId">The basket ID generated for the guest session before login.</param>
        /// <returns>The merged customer basket associated with the logged-in user.</returns>
        [Authorize]
        [HttpPost("merge")]
        public async Task<IActionResult> MergeBasket(string guestId)
        {
            if (!Guid.TryParse(guestId, out _))
                return BadRequest(ApiResponseFactory.BadRequest("Invalid Id format"));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponseFactory.Unauthorized("User not logged in"));

            var mergedBasket = await _basketService.MergeBasketsAsync(guestId, userId);

            return Ok(mergedBasket);
        }

    }
}
