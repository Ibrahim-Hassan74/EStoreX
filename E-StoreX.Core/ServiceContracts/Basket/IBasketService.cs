using EStoreX.Core.DTO.Basket;

namespace EStoreX.Core.ServiceContracts.Basket
{
    /// <summary>
    /// Interface for managing customer baskets.
    /// </summary>
    public interface IBasketService
    {
        /// <summary>
        /// Retrieves the basket of a specific customer by ID.
        /// </summary>
        /// <param name="id">Customer identifier.</param>
        /// <returns>The customer basket if found; otherwise, null.</returns>
        Task<CustomerBasketDTO?> GetBasketAsync(string id);

        /// <summary>
        /// Updates or creates a new customer basket.
        /// </summary>
        /// <param name="basket">Basket to update or create.</param>
        /// <returns>The updated customer basket.</returns>
        Task<CustomerBasketDTO?> UpdateBasketAsync(CustomerBasketDTO basket);

        /// <summary>
        /// Deletes the basket of a specific customer by ID.
        /// </summary>
        /// <param name="id">Customer identifier.</param>
        /// <returns>True if the basket was deleted; otherwise, false.</returns>
        Task<bool> DeleteBasketAsync(string id);
        /// <summary>
        /// Merges the guest basket (identified by a temporary guestId) with the basket 
        /// of the authenticated user (identified by userId).
        /// </summary>
        /// <param name="guestId">Basket ID created for the guest session before login.</param>
        /// <param name="userId">The authenticated user's ID.</param>
        /// <returns>The merged customer basket associated with the user, or null if merge fails.</returns>
        Task<CustomerBasketDTO?> MergeBasketsAsync(string guestId, string userId);

    }
}
