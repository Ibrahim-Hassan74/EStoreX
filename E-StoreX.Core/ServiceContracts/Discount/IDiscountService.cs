using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Discount.Request;

namespace EStoreX.Core.ServiceContracts.Discount
{
    /// <summary>
    /// Defines operations for managing discounts in the system, 
    /// including creation, retrieval, update, activation, expiration, and validation.
    /// </summary>
    public interface IDiscountService
    {
        /// <summary>
        /// Creates a new discount in the system.
        /// </summary>
        /// <param name="request">The details of the discount to create.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the result of the operation.</returns>
        Task<ApiResponse> CreateDiscountAsync(DiscountRequest request);

        /// <summary>
        /// Updates an existing discount with new details.
        /// </summary>
        /// <param name="id">The unique identifier of the discount to update.</param>
        /// <param name="request">The updated discount details.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the updated discount or an error if not found.</returns>
        Task<ApiResponse> UpdateDiscountAsync(Guid id, DiscountRequest request);

        /// <summary>
        /// Deletes a discount permanently from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the discount to delete.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating success or failure.</returns>
        Task<ApiResponse> DeleteDiscountAsync(Guid id);

        /// <summary>
        /// Retrieves a discount by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the discount.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the discount details or not found status.</returns>
        Task<ApiResponse> GetDiscountByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a discount by its unique code.
        /// </summary>
        /// <param name="code">The discount code.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the discount details or not found status.</returns>
        Task<ApiResponse> GetDiscountByCodeAsync(string code);

        /// <summary>
        /// Retrieves all discounts in the system, including active, expired, and upcoming.
        /// </summary>
        /// <returns>An <see cref="ApiResponse"/> containing a list of all discounts.</returns>
        Task<ApiResponse> GetAllDiscountsAsync();

        /// <summary>
        /// Retrieves all currently active discounts.
        /// </summary>
        /// <returns>An <see cref="ApiResponse"/> containing a list of active discounts.</returns>
        Task<ApiResponse> GetActiveDiscountsAsync();

        /// <summary>
        /// Retrieves all expired discounts.
        /// </summary>
        /// <returns>An <see cref="ApiResponse"/> containing a list of expired discounts.</returns>
        Task<ApiResponse> GetExpiredDiscountsAsync();

        /// <summary>
        /// Retrieves discounts that are scheduled but not yet started.
        /// </summary>
        /// <returns>An <see cref="ApiResponse"/> containing a list of upcoming discounts.</returns>
        Task<ApiResponse> GetNotStartedDiscountsAsync();

        /// <summary>
        /// Immediately activates a discount, making it available for use.
        /// </summary>
        /// <param name="id">The unique identifier of the discount to activate.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        Task<ApiResponse> ActivateDiscountAsync(Guid id);

        /// <summary>
        /// Immediately expires a discount, preventing further usage.
        /// </summary>
        /// <param name="id">The unique identifier of the discount to expire.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        Task<ApiResponse> ExpireDiscountAsync(Guid id);

        /// <summary>
        /// Updates the start and/or end date of a discount.
        /// </summary>
        /// <param name="id">The unique identifier of the discount.</param>
        /// <param name="startDate">The new start date for the discount.</param>
        /// <param name="endDate">The optional new end date for the discount.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the update operation.</returns>
        Task<ApiResponse> UpdateDiscountDatesAsync(Guid id, DateTime startDate, DateTime? endDate);

        /// <summary>
        /// Applies a discount code to a specific product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="code">The discount code to apply.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating success or failure of applying the discount.</returns>
        Task<ApiResponse> ApplyDiscountToProductAsync(Guid productId, string code);

        /// <summary>
        /// Validates a discount code to check if it is active and usable.
        /// </summary>
        /// <param name="code">The discount code to validate.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating whether the discount code is valid or not.</returns>
        Task<ApiResponse> ValidateDiscountCodeAsync(string code);
    }
}
