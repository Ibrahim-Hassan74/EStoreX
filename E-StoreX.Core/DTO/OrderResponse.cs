using EStoreX.Core.Enums;

namespace EStoreX.Core.DTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public ShippingAddressDTO ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; } = string.Empty;
        public List<OrderItemResponse> OrderItems { get; set; } = new();
    }
}
