using EStoreX.Core.Domain.Entities;

namespace EStoreX.Core.ServiceContracts
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId, Guid? deliveryMethodId);
        Task<CustomerBasket> GetPaymentIntentAsync(string basketId);
    }
}
