using EStoreX.Core.DTO.Ratings.Requests;
using EStoreX.Core.DTO.Ratings.Response;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_StoreX.API.Controllers.Public
{
    /// <summary>
    /// Handles all operations related to product ratings,
    /// including creating, updating, deleting, and retrieving ratings and summaries.
    /// </summary>
    public class RatingsController : CustomControllerBase
    {
        private readonly IRatingService _ratingService;

        /// <summary>
        /// Initializes a new instance of <see cref="RatingsController"/>.
        /// </summary>
        /// <param name="ratingService">The service responsible for rating operations.</param>
        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        /// <summary>
        /// Adds a new rating for a given product by the logged-in user.
        /// </summary>
        /// <param name="request">The rating details including score, comment, and product ID.</param>
        /// <returns>The created rating as a <see cref="RatingResponse"/>.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RatingResponse>> AddRating([FromBody] RatingAddRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _ratingService.AddRatingAsync(request, userId);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing rating owned by the logged-in user.
        /// </summary>
        /// <param name="id">The ID of the rating to update.</param>
        /// <param name="request">The updated rating details.</param>
        /// <returns>The updated rating as a <see cref="RatingResponse"/> or 404 if not found.</returns>
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<RatingResponse>> UpdateRating(Guid id, [FromBody] RatingUpdateRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _ratingService.UpdateRatingAsync(id, request, userId);
            if (result == null)
                return NotFound(ApiResponseFactory.NotFound("Rating not found or not owned by user."));
            return Ok(result);
        }

        /// <summary>
        /// Deletes a rating owned by the logged-in user.
        /// </summary>
        /// <param name="id">The ID of the rating to delete.</param>
        /// <returns>No content if successful, otherwise 404 if not found.</returns>
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _ratingService.DeleteRatingAsync(id, userId);
            if (!success)
                return NotFound(ApiResponseFactory.NotFound("Rating not found or not owned by user."));
            return NoContent();
        }

        /// <summary>
        /// Retrieves all ratings for a specific product.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>A list of ratings as <see cref="RatingResponse"/>.</returns>
        [HttpGet("product/{productId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RatingResponse>>> GetRatingsForProduct(Guid productId)
        {
            var result = await _ratingService.GetRatingsForProductAsync(productId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a summary of ratings for a specific product,
        /// including average score and total number of ratings.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>A <see cref="ProductRatingResponse"/> containing rating summary.</returns>
        [HttpGet("product/{productId:guid}/summary")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductRatingResponse>> GetProductRatingSummary(Guid productId)
        {
            var result = await _ratingService.GetProductRatingSummaryAsync(productId);
            return Ok(result);
        }
    }
}
