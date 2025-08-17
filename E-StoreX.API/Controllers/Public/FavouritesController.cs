using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Favourites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_StoreX.API.Controllers.Public
{
    /// <summary>
    /// Controller responsible for managing user's favourite products.
    /// Provides endpoints to add, remove, and retrieve favourites for the authenticated user.
    /// </summary>
    [Authorize]
    public class FavouritesController : CustomControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavouritesController"/> class.
        /// </summary>
        /// <param name="favouriteService">The service that handles favourite-related operations.</param>
        public FavouritesController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        /// <summary>
        /// Adds a product to the authenticated user's list of favourites.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to add.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the operation result.
        /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
        /// </returns>
        [HttpPost("{productId:guid}")]
        public async Task<IActionResult> AddToFavourite(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponseFactory.Unauthorized("User not found"));
            if(!Guid.TryParse(userId, out _))
                return Unauthorized(ApiResponseFactory.Unauthorized("Invalid user identifier"));
            var userIdGuid = Guid.Parse(userId);
            var response = await _favouriteService.AddToFavouriteAsync(userIdGuid, productId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Removes a product from the authenticated user's list of favourites.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the operation result.
        /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
        /// </returns>
        [HttpDelete("{productId:guid}")]
        public async Task<IActionResult> RemoveFromFavourite(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponseFactory.Unauthorized("User not found"));
            if (!Guid.TryParse(userId, out _))
                return Unauthorized(ApiResponseFactory.Unauthorized("Invalid user identifier"));
            var userIdGuid = Guid.Parse(userId);
            var response = await _favouriteService.RemoveFromFavouriteAsync(userIdGuid, productId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Retrieves all products in the authenticated user's favourites list.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the list of favourites.
        /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetUserFavourites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponseFactory.Unauthorized("User not found"));
            if (!Guid.TryParse(userId, out _))
                return Unauthorized(ApiResponseFactory.Unauthorized("Invalid user identifier"));
            var userIdGuid = Guid.Parse(userId);

            var response = await _favouriteService.GetUserFavouritesAsync(userIdGuid);
            return Ok(response);
        }
    }
}
