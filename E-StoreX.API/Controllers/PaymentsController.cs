using EStoreX.Core.Domain.Entities;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Handles payment-related operations such as creating or updating Stripe payment intents.
    /// </summary>
    /// <remarks>
    /// This controller is secured with [Authorize], meaning all endpoints require authenticated users.
    /// Typically called before placing an order to prepare or update the payment.
    /// </remarks>
    [Authorize]
    public class PaymentsController : CustomControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Creates or updates a Stripe payment intent for the specified basket and delivery method.
        /// </summary>
        /// <param name="basketId">The ID of the customer's basket. Cannot be null or empty.</param>
        /// <param name="deliveryMethodId">The ID of the selected delivery method. Optional.</param>
        /// <returns>
        /// Returns the updated <see cref="CustomerBasket"/> including the client secret for Stripe payment.
        /// Returns <c>BadRequest</c> if basketId is invalid.
        /// Returns <c>NotFound</c> if the basket does not exist.
        /// Returns <c>Ok</c> with the updated basket if successful.
        /// </returns>
        /// <remarks>
        /// This endpoint is called by the frontend before initiating payment through Stripe.
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId, Guid? deliveryMethodId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                return BadRequest("Basket ID cannot be null or empty.");
            }
            if (deliveryMethodId.HasValue && deliveryMethodId.Value == Guid.Empty)
            {
                return BadRequest("Invalid delivery method ID.");
            }
            var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, deliveryMethodId);
            if (basket == null)
            {
                return NotFound("Basket not found.");
            }
            return Ok(basket);
        }

    }
}
