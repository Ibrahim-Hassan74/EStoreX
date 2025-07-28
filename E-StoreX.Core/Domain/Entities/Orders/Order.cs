using EStoreX.Core.Enums;

namespace EStoreX.Core.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order(string buyerEmail, decimal subTotal, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, IEnumerable<OrderItem> orderItems)
        {
            BuyerEmail = buyerEmail;
            SubTotal = subTotal;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
        }
        public Order() { }
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Pending;
        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Price;
        }
    }
}
