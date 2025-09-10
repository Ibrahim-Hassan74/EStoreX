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
        Task<CustomerBasketDTO?> AddItemToBasketAsync(BasketAddRequest basket);

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
        /// <summary>
        /// Removes a specific item from the customer's basket.
        /// </summary>
        /// <param name="basketId">
        /// The unique identifier of the basket (must be a valid GUID).
        /// </param>
        /// <param name="productId">
        /// The unique identifier of the product to remove.
        /// </param>
        /// <returns>
        /// A <see cref="CustomerBasketDTO"/> representing the updated basket,  
        /// or <c>null</c> if the basket or item does not exist.
        /// </returns>
        Task<CustomerBasketDTO?> RemoveItemAsync(string basketId, Guid productId);

        /// <summary>
        /// Decreases the quantity of a specific item in the customer's basket by 1.  
        /// If the quantity reaches zero, the item will be removed from the basket.
        /// </summary>
        /// <param name="basketId">
        /// The unique identifier of the basket (must be a valid GUID).
        /// </param>
        /// <param name="productId">
        /// The unique identifier of the product to decrease the quantity for.
        /// </param>
        /// <returns>
        /// A <see cref="CustomerBasketDTO"/> representing the updated basket,  
        /// or <c>null</c> if the basket or item does not exist.
        /// </returns>
        Task<CustomerBasketDTO?> DecreaseItemQuantityAsync(string basketId, Guid productId);
        /// <summary>
        /// Increases the quantity of a specific item in the customer's basket.
        /// </summary>
        /// <param name="basketId">The identifier of the basket.</param>
        /// <param name="productId">The identifier of the product whose quantity will be increased.</param>
        /// <returns>
        /// A <see cref="CustomerBasketDTO"/> representing the updated basket 
        /// if the basket and product exist; otherwise, <c>null</c>.
        /// </returns>
        Task<CustomerBasketDTO?> IncreaseItemQuantityAsync(string basketId, Guid productId);
        /// <summary>
        /// Applies a discount code to the specified customer's basket.
        /// </summary>
        /// <param name="basketId">The unique identifier of the basket where the discount should be applied.</param>
        /// <param name="discountCode">The discount code provided by the customer.</param>
        /// <returns>
        /// A <see cref="CustomerBasketDTO"/> representing the updated basket with the discount applied,  
        /// or <c>null</c> if the basket or discount code is not found.
        /// </returns>
        Task<CustomerBasketDTO?> ApplyDiscountAsync(string basketId, string discountCode);
    }
}
