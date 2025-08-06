using EStoreX.Core.Domain.Entities;
using EStoreX.Core.Domain.Entities.Orders;

namespace EStoreX.Core.ServiceContracts
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
    }
}
