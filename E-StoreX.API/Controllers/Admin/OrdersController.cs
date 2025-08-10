using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Provides functionality for managing and processing orders within the administrative context.
    /// </summary>
    /// <remarks>This controller inherits from <see cref="AdminControllerBase"/> and is intended for use in
    /// administrative scenarios where order-related operations are required. It serves as a base for implementing
    /// actions related to order management.</remarks>
    public class OrdersController : AdminControllerBase
    {
        private readonly IOrderService _orderService;
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class with the specified order service.
        /// </summary>
        /// <param name="orderService">The service used to manage order-related operations.</param>
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Retrieves all orders from the system.
        /// </summary>
        /// <returns>All orders</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }
    }
}
