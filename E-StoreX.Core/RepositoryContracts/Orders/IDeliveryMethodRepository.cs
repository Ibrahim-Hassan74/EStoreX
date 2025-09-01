using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Orders
{
    /// <summary>
    /// Repository interface for handling data operations related to <see cref="DeliveryMethod"/>.
    /// Extends the generic repository with delivery-method-specific operations.
    /// </summary>
    public interface IDeliveryMethodRepository : IGenericRepository<DeliveryMethod>
    {
        /// <summary>
        /// Retrieves a <see cref="DeliveryMethod"/> by its unique name.
        /// </summary>
        /// <param name="name">The name of the delivery method to search for.</param>
        /// <returns>
        /// The matching <see cref="DeliveryMethod"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<DeliveryMethod?> GetByNameAsync(string name);
    }
}
