using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Orders
{
    public interface IDeliveryMethodRepository : IGenericRepository<DeliveryMethod>
    {
        Task<DeliveryMethod?> GetByNameAsync(string name);
    }
}
