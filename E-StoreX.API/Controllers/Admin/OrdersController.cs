using Asp.Versioning;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Orders.Responses;
using EStoreX.Core.ServiceContracts.Orders;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Provides functionality for managing and processing orders within the administrative context.
    /// </summary>
    /// <remarks>This controller inherits from <see cref="AdminControllerBase"/> and is intended for use in
    /// administrative scenarios where order-related operations are required. It serves as a base for implementing
    /// actions related to order management.</remarks>
    [ApiVersion(2.0)]
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
        /// <returns>
        /// Returns <see cref="IEnumerable{OrderResponse}"/> containing all orders if found.  
        /// Returns <see cref="UnauthorizedResult"/> if the user is not authenticated.  
        /// Returns <see cref="NotFoundResult"/> if no orders exist.  
        /// Returns <see cref="StatusCodeResult"/> 500 if an unexpected error occurs.
        /// </returns>
        /// <response code="200">Orders retrieved successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }
    }
}
