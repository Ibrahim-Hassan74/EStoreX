using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Products.Responses;

namespace EStoreX.Core.ServiceContracts.Favourites
{
    /// <summary>
    /// Defines operations for managing a user's favourite products at the service level.
    /// </summary>
    public interface IFavouriteService
    {
        /// <summary>
        /// Adds a product to the user's list of favourites.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// An <see cref="ApiResponse"/> indicating the result of the operation.
        /// Contains a success flag, message, and optional error details.
        /// </returns>
        Task<ApiResponse> AddToFavouriteAsync(Guid userId, Guid productId);

        /// <summary>
        /// Removes a product from the user's list of favourites.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// An <see cref="ApiResponse"/> indicating whether the product was successfully removed
        /// or if it was not found in the user's favourites.
        /// </returns>
        Task<ApiResponse> RemoveFromFavouriteAsync(Guid userId, Guid productId);

        /// <summary>
        /// Retrieves all products that the user has marked as favourites.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// An <see cref="ProductResponse"/> containing a list of the user's favourite products 
        /// if any are found; otherwise, an empty list.
        /// </returns>
        Task<List<ProductResponse>> GetUserFavouritesAsync(Guid userId);
    }
}
