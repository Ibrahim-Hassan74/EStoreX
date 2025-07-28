using EStoreX.Core.Domain.Entities.Orders;

namespace EStoreX.Core.RepositoryContracts
{
    /// <summary>
    /// Represents the data access contract for managing orders.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Persists a new order to the database.
        /// </summary>
        /// <param name="order">The order to create.</param>
        /// <returns>The created order entity.</returns>
        Task<Order> CreateOrderAsync(Order order);

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
    }
}
