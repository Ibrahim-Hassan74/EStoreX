using Domain.Entities.Baskets;

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
        Task<CustomerBasket?> GetBasketAsync(string id);

        /// <summary>
        /// Updates or creates a new customer basket.
        /// </summary>
        /// <param name="basket">Basket to update or create.</param>
        /// <returns>The updated customer basket.</returns>
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);

        /// <summary>
        /// Deletes the basket of a specific customer by ID.
        /// </summary>
        /// <param name="id">Customer identifier.</param>
        /// <returns>True if the basket was deleted; otherwise, false.</returns>
        Task<bool> DeleteBasketAsync(string id);
    }
}
