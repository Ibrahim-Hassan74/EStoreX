using EStoreX.Core.DTO.Ratings.Requests;
using EStoreX.Core.DTO.Ratings.Response;

namespace EStoreX.Core.ServiceContracts.Ratings
{
    /// <summary>
    /// Defines service operations for managing product ratings,
    /// including creation, modification, deletion, and retrieval.
    /// </summary>
    public interface IRatingService
    {
        /// <summary>
        /// Adds a new rating for a product by a specific user.
        /// </summary>
        /// <param name="request">The rating details to be added.</param>
        /// <param name="userId">The unique identifier of the user submitting the rating.</param>
        /// <returns>
        /// A <see cref="RatingResponse"/> representing the newly created rating.
        /// </returns>
        Task<RatingResponse> AddRatingAsync(RatingAddRequest request, Guid userId);

        /// <summary>
        /// Updates an existing rating if it belongs to the specified user.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating to update.</param>
        /// <param name="request">The updated rating details.</param>
        /// <param name="userId">The unique identifier of the user attempting the update.</param>
        /// <returns>
        /// A <see cref="RatingResponse"/> containing the updated rating if successful; otherwise <c>null</c>.
        /// </returns>
        Task<RatingResponse?> UpdateRatingAsync(Guid ratingId, RatingUpdateRequest request, Guid userId);

        /// <summary>
        /// Deletes a rating if it belongs to the specified user.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating to delete.</param>
        /// <param name="userId">The unique identifier of the user attempting the deletion.</param>
        /// <returns>
        /// <c>true</c> if the rating was successfully deleted; otherwise <c>false</c>.
        /// </returns>
        Task<bool> DeleteRatingAsync(Guid ratingId, Guid userId);

        /// <summary>
        /// Retrieves all ratings submitted for a specific product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// A collection of <see cref="RatingResponse"/> representing the product's ratings.
        /// </returns>
        Task<IEnumerable<RatingResponse>> GetRatingsForProductAsync(Guid productId);

        /// <summary>
        /// Retrieves a summary of ratings for a product, including average score and total count.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// A <see cref="ProductRatingResponse"/> containing rating summary information.
        /// </returns>
        Task<ProductRatingResponse> GetProductRatingSummaryAsync(Guid productId);
    }

}
