using Domain.Entities.Product;
using EStoreX.Core.Domain.Entities.Favourites;

namespace EStoreX.Core.RepositoryContracts.Favourites
{
    /// <summary>
    /// Defines methods for managing user favourite products in the repository.
    /// </summary>
    public interface IFavouriteRepository
    {
        /// <summary>
        /// Adds a product to the user's list of favourites.
        /// </summary>
        /// <param name="favourite">The favourite entity containing the user and product information.</param>
        /// <returns>The created <see cref="Favourite"/> entity.</returns>
        Task<Favourite> AddToFavouriteAsync(Favourite favourite);

        /// <summary>
        /// Removes a product from the user's list of favourites.
        /// </summary>
        /// <param name="favourite">The favourite entity containing the user and product information.</param>
        /// <returns><c>true</c> if the favourite was removed successfully; otherwise, <c>false</c>.</returns>
        Task<bool> RemoveFromFavouriteAsync(Favourite favourite);


        /// <summary>
        /// Checks if a product is already in the user's list of favourites.
        /// </summary>
        /// <param name="favourite">The favourite entity containing the user and product information.</param>
        /// <returns><c>true</c> if the product is in the user's favourites; otherwise, <c>false</c>.</returns>
        Task<bool> IsFavouriteAsync(Favourite favourite);

        /// <summary>
        /// Retrieves all favourite products for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of <see cref="Product"/> entities favourited by the user.</returns>
        Task<List<Product>> GetUserFavouritesAsync(Guid userId);
    }
}
