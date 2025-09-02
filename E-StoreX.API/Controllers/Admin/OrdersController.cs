using Asp.Versioning;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Orders.Responses;
using EStoreX.Core.Enums;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Common;
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
        private readonly IExportService _exportService;
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class with the specified order service.
        /// </summary>
        /// <param name="orderService">The service used to manage order-related operations.</param>
        /// <param name="exportService">Service to manage files.</param>
        public OrdersController(IOrderService orderService, IExportService exportService)
        {
            _orderService = orderService;
            _exportService = exportService;
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
        /// <summary>
        /// Exports all orders into the specified file format.
        /// </summary>
        /// <param name="type">
        /// The type of export format.  
        /// Supported values are:  
        /// <list type="bullet">
        ///   <item><description><see cref="ExportType.Csv"/> → Comma Separated Values file (.csv)</description></item>
        ///   <item><description><see cref="ExportType.Excel"/> → Microsoft Excel file (.xlsx)</description></item>
        ///   <item><description><see cref="ExportType.Pdf"/> → Portable Document Format file (.pdf)</description></item>
        /// </list>
        /// </param>
        /// <returns>
        /// A downloadable file in the selected export format.  
        /// Returns <see cref="BadRequestResult"/> if the format is not supported.
        /// </returns>
        /// <response code="200">orders exported successfully in the requested format.</response>
        /// <response code="400">Unsupported export type requested.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("export/{type}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Export(ExportType type)
        {
            var orders = await _orderService.GetAllOrders();

            return type switch
            {
                ExportType.Csv => File(_exportService.ExportToCsv(orders), "text/csv", "orders.csv"),
                ExportType.Excel => File(_exportService.ExportToExcel(orders), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "orders.xlsx"),
                ExportType.Pdf => File(_exportService.ExportToPdf(orders), "application/pdf", "orders.pdf"),
                _ => BadRequest(ApiResponseFactory.BadRequest("Unsupported export type"))
            };
        }
    }
}
