using EStoreX.Core.DTO;
using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling order-related operations.
    /// Requires the user to be authenticated.
    /// </summary>
    [Authorize]
    public class OrdersController : CustomControllerBase
    {
        private readonly IOrderService _orderService;
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderService">The service that handles order-related operations.</param>
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order for the authenticated user.
        /// </summary>
        /// <param name="order">The order data to be created.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with the created order details if successful;
        /// otherwise, returns <see cref="BadRequestObjectResult"/> if the user email is not found.
        /// </returns>
        /// <response code="200">Returns the newly created order.</response>
        /// <response code="400">If the user email is not present in the JWT token.</response>
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderAddRequest order)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("User email not found in token.");

            var createdOrder = await _orderService.CreateOrdersAsync(order, email);

            return Ok(createdOrder);
        }
        /// <summary>
        /// Retrieves all orders associated with the currently authenticated user.
        /// </summary>
        /// <returns>A list of <see cref="OrderResponse"/> objects representing the user's orders.</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return NotFound();
            var orderResponse = await _orderService.GetAllOrdersAsync(email);
            return Ok(orderResponse);
        }

        /// <summary>
        /// Retrieves a specific order by its ID for the currently authenticated user.
        /// </summary>
        /// <param name="Id">The unique identifier of the order.</param>
        /// <returns>An <see cref="OrderResponse"/> object representing the order, or NotFound if not found.</returns>
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetOrdersById(Guid Id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return NotFound();
            var orderResponse = await _orderService.GetOrderByIdAsync(Id, email);
            return Ok(orderResponse);
        }

        /// <summary>
        /// Retrieves all available delivery methods.
        /// </summary>
        /// <returns>A list of <see cref="DeliveryMethodResponse"/> objects representing delivery options.</returns>
        [HttpGet("delivery-methods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var methods = await _orderService.GetDeliveryMethodAsync();
            return Ok(methods);
        }

    }
}
