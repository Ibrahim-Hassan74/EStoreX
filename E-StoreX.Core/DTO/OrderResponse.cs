using EStoreX.Core.Enums;

namespace EStoreX.Core.DTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public Status Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public ShippingAddressDTO ShippingAddress { get; set; }
        public DeliveryMethodResponse DeliveryMethod { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new();
    }
}
