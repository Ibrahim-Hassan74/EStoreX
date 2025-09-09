using Asp.Versioning;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Discount.Request;
using EStoreX.Core.DTO.Discounts.Responses;
using EStoreX.Core.ServiceContracts.Discount;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// Provides endpoints for managing discounts in the system.
    /// This controller is intended for administrators only.
    /// </summary>
    [ApiVersion(2.0)]
    public class DiscountsController : AdminControllerBase
    {
        private readonly IDiscountService _discountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountsController"/> class.
        /// </summary>
        /// <param name="discountService">Service for handling discount-related operations.</param>
        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        /// <summary>
        /// Creates a new discount.
        /// </summary>
        /// <param name="request">The discount details to be created.</param>
        /// <returns>Details of the created discount.</returns>
        /// <response code="200">Discount created successfully.</response>
        /// <response code="400">Invalid discount details were provided.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<DiscountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] DiscountRequest request)
        {
            var result = await _discountService.CreateDiscountAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        /// <param name="id">The ID of the discount to update.</param>
        /// <param name="request">The updated discount details.</param>
        /// <returns>The updated discount details.</returns>
        /// <response code="200">Discount updated successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<DiscountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DiscountRequest request)
        {
            var result = await _discountService.UpdateDiscountAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Deletes an existing discount.
        /// </summary>
        /// <param name="id">The ID of the discount to delete.</param>
        /// <returns>Status of the delete operation.</returns>
        /// <response code="200">Discount deleted successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _discountService.DeleteDiscountAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Activates a discount immediately.
        /// </summary>
        /// <param name="id">The ID of the discount to activate.</param>
        /// <returns>Status of the operation.</returns>
        /// <response code="200">Discount activated successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpPatch("{id}/activate")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _discountService.ActivateDiscountAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Expires a discount immediately.
        /// </summary>
        /// <param name="id">The ID of the discount to expire.</param>
        /// <returns>Status of the operation.</returns>
        /// <response code="200">Discount expired successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpPatch("{id}/expire")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Expire(Guid id)
        {
            var result = await _discountService.ExpireDiscountAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Updates the start and end dates of a discount.
        /// </summary>
        /// <param name="id">The ID of the discount to update.</param>
        /// <param name="request">The request containing the new start and end dates.</param>
        /// <returns>Status of the operation.</returns>
        /// <response code="200">Discount dates updated successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpPatch("{id}/dates")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDates(Guid id, [FromBody] UpdateDiscountDatesRequest request)
        {
            var result = await _discountService.UpdateDiscountDatesAsync(id, request.StartDate, request.EndDate);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a discount by its ID.
        /// </summary>
        /// <param name="id">The discount ID.</param>
        /// <returns>The discount details.</returns>
        /// <response code="200">Discount retrieved successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<DiscountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _discountService.GetDiscountByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves a discount by its unique code.
        /// </summary>
        /// <param name="code">The discount code.</param>
        /// <returns>The discount details.</returns>
        /// <response code="200">Discount retrieved successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpGet("code/{code}")]
        [ProducesResponseType(typeof(ApiResponseWithData<DiscountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _discountService.GetDiscountByCodeAsync(code);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all discounts.
        /// </summary>
        /// <returns>List of all discounts.</returns>
        /// <response code="200">Discounts retrieved successfully.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<DiscountResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _discountService.GetAllDiscountsAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all currently active discounts.
        /// </summary>
        /// <returns>List of active discounts.</returns>
        /// <response code="200">Active discounts retrieved successfully.</response>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponseWithData<List<DiscountResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive()
        {
            var result = await _discountService.GetActiveDiscountsAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves all expired discounts.
        /// </summary>
        /// <returns>List of expired discounts.</returns>
        /// <response code="200">Expired discounts retrieved successfully.</response>
        [HttpGet("expired")]
        [ProducesResponseType(typeof(ApiResponseWithData<List<DiscountResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExpired()
        {
            var result = await _discountService.GetExpiredDiscountsAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Retrieves discounts that have not started yet.
        /// </summary>
        /// <returns>List of upcoming discounts.</returns>
        /// <response code="200">Upcoming discounts retrieved successfully.</response>
        [HttpGet("upcoming")]
        [ProducesResponseType(typeof(ApiResponseWithData<List<DiscountResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotStarted()
        {
            var result = await _discountService.GetNotStartedDiscountsAsync();
            return StatusCode(result.StatusCode, result);
        }
    }
}
