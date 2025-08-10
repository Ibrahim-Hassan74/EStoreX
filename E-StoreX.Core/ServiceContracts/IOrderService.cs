using EStoreX.Core.DTO;

namespace EStoreX.Core.ServiceContracts
{
    /// <summary>
    /// Represents the contract for order-related operations.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order for the specified user.
        /// </summary>
        /// <param name="order">Order creation request details.</param>
        /// <param name="buyerEmail">Email of the user placing the order.</param>
        /// <returns>The created order as a response DTO.</returns>
        Task<OrderResponse> CreateOrdersAsync(OrderAddRequest order, string buyerEmail);

        /// <summary>
        /// Retrieves all orders placed by a specific user.
        /// </summary>
        /// <param name="buyerEmail">Email of the user.</param>
        /// <returns>List of order responses.</returns>
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(string buyerEmail);

        /// <summary>
        /// Retrieves a specific order by its ID for a given user.
        /// </summary>
        /// <param name="Id">The ID of the order.</param>
        /// <param name="buyerEmail">Email of the user.</param>
        /// <returns>The requested order if found; otherwise, null.</returns>
        Task<OrderResponse> GetOrderByIdAsync(Guid Id, string buyerEmail);

        /// <summary>
        /// Retrieves all available delivery methods.
        /// </summary>
        /// <returns>List of available delivery method responses.</returns>
        Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodAsync();
        /// <summary>
        /// Retrieves all orders in the system, regardless of the user.
        /// </summary>
        /// <returns>All Orders</returns>
        Task<IEnumerable<OrderResponse>> GetAllOrders();
    }
}
