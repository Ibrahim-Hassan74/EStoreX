using Domain.Entities.Baskets;
using Domain.Entities.Product;

namespace EStoreX.Core.ServiceContracts.Common
{
    /// <summary>
    /// Represents the contract for payment-related operations including payment intent creation and order status updates.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Creates or updates a Stripe payment intent for the given basket.
        /// </summary>
        /// <param name="basketId">The unique identifier of the basket.</param>
        /// <param name="deliveryMethodId">Optional delivery method ID to calculate shipping costs.</param>
        /// <returns>The updated customer basket with payment intent details.</returns>
        Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId, Guid? deliveryMethodId);

        /// <summary>
        /// Updates the status of the order to PaymentFailed when the payment fails.
        /// </summary>
        /// <param name="paymentIntentId">The Stripe PaymentIntent ID associated with the order.</param>
        /// <returns>true or false if not found.</returns>
        Task<bool> UpdateOrderFailedAsync(string? paymentIntentId);

        /// <summary>
        /// Updates the status of the order to PaymentReceived when the payment is successful.
        /// </summary>
        /// <param name="paymentIntentId">The Stripe PaymentIntent ID associated with the order.</param>
        /// <returns>true or false if not found.</returns>
        Task<bool> UpdateOrderSuccessAsync(string? paymentIntentId);
        /// <summary>
        /// Calculates the discounted unit price and total discount amount for a given product in the context of a customer's basket.
        /// </summary>
        /// <param name="product">The product for which the discount should be evaluated.</param>
        /// <param name="quantity">The quantity of the product in the basket.</param>
        /// <param name="basket">The customer's basket, which may contain applicable discounts or promotions.</param>
        /// <returns>
        /// A tuple containing:  
        /// <list type="bullet">
        ///   <item><description><c>unitPrice</c>: The final unit price of the product after applying discounts.</description></item>
        ///   <item><description><c>discountAmount</c>: The total discount amount applied based on the product and quantity.</description></item>
        /// </list>
        /// </returns>
        Task<(decimal unitPrice, decimal discountAmount)> GetDiscountedPriceAsync(Product product, int quantity, CustomerBasket basket);
    }
}
