using Asp.Versioning;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Orders.Requests;
using EStoreX.Core.DTO.Orders.Responses;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Orders;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// DeliveryMethodController handles admin operations related to delivery methods.
    /// </summary>
    [Route("api/v{version:apiVersion}/admin/delivery-method")]
    [ApiVersion(2.0)]
    public class DeliveryMethodController : AdminControllerBase
    {
        private readonly IDeliveryMethodService _deliveryMethod;
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryMethodController"/> class.
        /// </summary>
        public DeliveryMethodController(IDeliveryMethodService deliveryMethod)
        {
            _deliveryMethod = deliveryMethod;
        }
        /// <summary>
        /// Creates a new delivery method.
        /// </summary>
        /// <param name="request">The delivery method details.</param>
        /// <returns>The created delivery method.</returns>
        /// <response code="201">Delivery method created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Unauthorized user.</response>
        [HttpPost]
        [ProducesResponseType(typeof(DeliveryMethodResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] DeliveryMethodRequest request)
        {
            var created = await _deliveryMethod.CreateAsync(request);
            return Ok(created);
        }

        /// <summary>
        /// Updates an existing delivery method.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery method to update.</param>
        /// <param name="request">The updated delivery method details.</param>
        /// <returns>The updated delivery method.</returns>
        /// <response code="200">Delivery method updated successfully.</response>
        /// <response code="404">Delivery method not found.</response>
        /// <response code="401">Unauthorized user.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DeliveryMethodResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DeliveryMethodRequest request)
        {
            var updated = await _deliveryMethod.UpdateAsync(id, request);
            if (updated == null)
                return NotFound(ApiResponseFactory.NotFound());
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a delivery method by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery method to delete.</param>
        /// <returns>No content if deletion succeeded.</returns>
        /// <response code="204">Delivery method deleted successfully.</response>
        /// <response code="404">Delivery method not found.</response>
        /// <response code="401">Unauthorized user.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _deliveryMethod.DeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponseFactory.NotFound());
            return NoContent();
        }
    }
}