using Domain.Entities.Baskets;

namespace EStoreX.Core.RepositoryContracts.Basket
{
    /// <summary>
    /// Defines repository operations for managing customer baskets in the data store.
    /// </summary>
    public interface ICustomerBasketRepository
    {
        /// <summary>
        /// Retrieves a customer basket by its unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the basket.</param>
        /// <returns>The customer basket if found; otherwise, null.</returns>
        Task<CustomerBasket?> GetBasketAsync(string Id);

        /// <summary>
        /// Updates an existing customer basket or creates a new one if it doesn't exist.
        /// </summary>
        /// <param name="basket">The customer basket to update or create.</param>
        /// <returns>The updated or newly created customer basket.</returns>
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);

        /// <summary>
        /// Deletes a customer basket by its unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the basket to delete.</param>
        /// <returns>True if the basket was deleted; otherwise, false.</returns>
        Task<bool> DeleteBasketAsync(string Id);
    }
}
