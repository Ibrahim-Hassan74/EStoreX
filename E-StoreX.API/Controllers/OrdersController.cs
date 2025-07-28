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


    }
}
