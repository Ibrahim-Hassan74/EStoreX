using Domain.Entities.Product;

namespace EStoreX.Core.RepositoryContracts
{
    /// <summary>
    /// Defines methods for managing user favourite products in the repository.
    /// </summary>
    public interface IFavouriteRepository
    {
        /// <summary>
        /// Adds a product to the user's list of favourites.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        Task AddToFavouriteAsync(string userId, int productId);

        /// <summary>
        /// Removes a product from the user's list of favourites.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        Task RemoveFromFavouriteAsync(string userId, int productId);

        /// <summary>
        /// Checks if a product is already in the user's list of favourites.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>True if the product is in the user's favourites; otherwise, false.</returns>
        Task<bool> IsFavouriteAsync(string userId, int productId);

        /// <summary>
        /// Retrieves all favourite products for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of products favourited by the user.</returns>
        Task<List<Product>> GetUserFavouritesAsync(string userId);
    }

}
