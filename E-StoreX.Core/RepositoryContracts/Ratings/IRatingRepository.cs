using EStoreX.Core.Domain.Entities.Rating;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Ratings
{
    /// <summary>
    /// Defines repository operations for managing <see cref="Rating"/> entities.
    /// Provides methods to retrieve product ratings, calculate averages, 
    /// and get a specific user's rating for a product.
    /// </summary>
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        /// <summary>
        /// Retrieves all ratings associated with a specific product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>A collection of <see cref="Rating"/> entities.</returns>
        Task<IEnumerable<Rating>> GetRatingsByProductAsync(Guid productId);

        /// <summary>
        /// Calculates the average rating score for a given product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// A <see cref="double"/> representing the average score. 
        /// Returns 0 if the product has no ratings.
        /// </returns>
        Task<double> GetAverageRatingAsync(Guid productId);

        /// <summary>
        /// Retrieves the rating submitted by a specific user for a given product, if it exists.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A <see cref="Rating"/> entity if the user has rated the product; otherwise <c>null</c>.
        /// </returns>
        Task<Rating?> GetUserRatingForProductAsync(Guid productId, Guid userId);
    }
}
