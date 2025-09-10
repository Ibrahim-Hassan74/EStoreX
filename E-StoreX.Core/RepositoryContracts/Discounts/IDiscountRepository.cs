using EStoreX.Core.RepositoryContracts.Common;
using Domain.Entities.Product;

namespace EStoreX.Core.RepositoryContracts.Discounts
{
    /// <summary>
    /// Repository interface for managing <see cref="Discount"/> entities.
    /// Provides methods for retrieving, filtering, and searching discounts based on status or code.
    /// </summary>
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        /// <summary>
        /// Retrieves a discount by its unique code.
        /// </summary>
        /// <param name="code">The discount code.</param>
        /// <returns>The <see cref="Discount"/> matching the code, or null if not found.</returns>
        Task<Discount?> GetByCodeAsync(string code);

        /// <summary>
        /// Retrieves all currently active discounts based on start/end dates and usage limits.
        /// </summary>
        /// <returns>A collection of active <see cref="Discount"/> entities.</returns>
        Task<IEnumerable<Discount>> GetActiveDiscountsAsync();

        /// <summary>
        /// Retrieves all expired discounts that are past their end date or reached usage limits.
        /// </summary>
        /// <returns>A collection of expired <see cref="Discount"/> entities.</returns>
        Task<IEnumerable<Discount>> GetExpiredDiscountsAsync();

        /// <summary>
        /// Retrieves all discounts that have not started yet (start date is in the future).
        /// </summary>
        /// <returns>A collection of upcoming <see cref="Discount"/> entities.</returns>
        Task<IEnumerable<Discount>> GetNotStartedDiscountsAsync();

        /// <summary>
        /// Retrieves an active discount by its code, considering dates and usage limits.
        /// </summary>
        /// <param name="code">The discount code.</param>
        /// <returns>The active <see cref="Discount"/> matching the code, or null if none found.</returns>
        Task<Discount?> GetActiveDiscountByCodeAsync(string code);
    }
}
