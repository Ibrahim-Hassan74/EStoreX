using Stripe;
using E_StoreX.API.Helper;
using EStoreX.Core.DTO.Common;
using Microsoft.AspNetCore.Mvc;
using EStoreX.Core.Domain.Options;
using Microsoft.Extensions.Options;
using EStoreX.Core.Domain.Entities;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;

namespace E_StoreX.API.Controllers.Public
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
        private readonly ILogger<PaymentsController> _logger;
        private readonly string _signingSecret;
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentsController"/> class.
        /// Handles payment operations including creating payment intents and processing webhooks.
        /// </summary>
        /// <param name="paymentService">Service for handling payment-related business logic.</param>
        /// <param name="options">Stripe configuration options (e.g., signing secret).</param>
        /// <param name="logger">Logger instance for logging payment events and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown if Stripe signing secret is not provided in configuration.</exception>
        public PaymentsController(IPaymentService paymentService, IOptions<StripeSettings> options, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _signingSecret = options.Value.SigningSecret ?? throw new ArgumentNullException(nameof(options), "Stripe signing secret cannot be null.");
            _logger = logger;
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
        public async Task<ActionResult<PaymentIntentDTO>> CreateOrUpdatePaymentIntent(string basketId, Guid? deliveryMethodId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                return BadRequest(ApiResponseFactory.BadRequest("Basket ID cannot be null or empty."));
            }
            if (deliveryMethodId.HasValue && deliveryMethodId.Value == Guid.Empty)
            {
                return BadRequest(ApiResponseFactory.BadRequest("Invalid delivery method ID."));
            }
            var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, deliveryMethodId);
            if (basket == null)
            {
                return NotFound(ApiResponseFactory.NotFound("Basket not found."));
            }
            var paymentIntentDto = new PaymentIntentDTO
            {
                PaymentIntentId = basket.PaymentIntentId,
                ClientSecret = basket.ClientSecret,
            };
            return Ok(paymentIntentDto);
        }

        /// <summary>
        /// Handles incoming Stripe webhook events to update the order status based on the payment result.
        /// </summary>
        /// <remarks>
        /// This endpoint is called by Stripe to notify about payment intent status changes such as success or failure.
        /// It processes the webhook payload, validates the Stripe signature, and updates the related order in the system.
        /// 
        /// Expected events:
        /// <list type="bullet">
        ///   <item>
        ///     <term>payment_intent.succeeded</term>
        ///     <description>Updates the order status to PaymentReceived.</description>
        ///   </item>
        ///   <item>
        ///     <term>payment_intent.payment_failed</term>
        ///     <description>Updates the order status to PaymentFailed.</description>
        ///   </item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// Returns <see cref="OkResult"/> (200) if the webhook is processed successfully.
        /// Returns <see cref="BadRequestResult"/> (400) if there's an error validating or processing the event.
        /// </returns>
        [HttpPost("webhook")]
        public async Task<IActionResult> UpdateStatusWithStripe()

        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _signingSecret,
                    throwOnApiVersionMismatch: false
                );

                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    var intent = stripeEvent.Data.Object as PaymentIntent;
                    var result = await _paymentService.UpdateOrderFailedAsync(intent?.Id);

                    if (!result)
                    {
                        _logger.LogWarning("Failed to update order as PaymentFailed for PaymentIntent: {PaymentIntentId}", intent?.Id);
                    }
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var intent = stripeEvent.Data.Object as PaymentIntent;
                    var result = await _paymentService.UpdateOrderSuccessAsync(intent?.Id);

                    if (!result)
                    {
                        _logger.LogWarning("Failed to update order as PaymentReceived for PaymentIntent: {PaymentIntentId}", intent?.Id);
                    }
                }
                else
                {
                    _logger.LogInformation("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe webhook error.");
                return BadRequest(ApiResponseFactory.BadRequest());
            }
        }

    }
}
