using Domain.Entities;
using EStoreX.Core.Domain.Entities.Orders;

namespace EStoreX.Core.DTO
{
    public class OrderDTO
    {
        public Guid DeliveryMethodId { get; set; }
        public string BasketId { get; set; }
        public ShippingAddressDTO ShippingAddress { get; set; }

    }
}
