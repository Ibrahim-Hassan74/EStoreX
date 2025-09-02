using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Orders
{
    /// <summary>
    /// Represents the data access contract for managing orders.
    /// </summary>
    public interface IOrderRepository : IGenericRepository<Order>
    {
        /// <summary>
        /// Retrieves all orders for a specific buyer.
        /// </summary>
        /// <param name="buyerEmail">The email of the buyer.</param>
        /// <returns>List of order entities.</returns>
        Task<IEnumerable<Order>> GetOrdersByBuyerEmailAsync(string buyerEmail);

        /// <summary>
        /// Retrieves a specific order by ID and buyer email.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="buyerEmail">The email of the buyer.</param>
        /// <returns>The order if found; otherwise, null.</returns>
        Task<Order?> GetOrderByIdAsync(Guid orderId, string buyerEmail);

        /// <summary>
        /// Retrieves all available delivery methods.
        /// </summary>
        /// <returns>List of delivery method entities.</returns>
        Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodsAsync();

        /// <summary>
        /// Retrieves a specific delivery method by ID.
        /// </summary>
        /// <param name="id">The ID of the delivery method.</param>
        /// <returns>The delivery method if found; otherwise, null.</returns>
        Task<DeliveryMethod?> GetDeliveryMethodByIdAsync(Guid id);
        /// <summary>
        /// Retrieves an order using the specified PaymentIntentId.
        /// </summary>
        /// <param name="paymentIntentId">The PaymentIntent ID associated with the order.</param>
        /// <returns>The order if found; otherwise, null.</returns>
        Task<Order?> GetOrderByPaymentIntentIdAsync(string? paymentIntentId);
        /// <summary>
        /// Retrieves all orders within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range (inclusive).</param>
        /// <param name="endDate">The end date of the range (inclusive).</param>
        /// <returns>A collection of <see cref="Order"/> entities within the range.</returns>
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
