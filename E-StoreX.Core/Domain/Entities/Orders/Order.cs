using Domain.Entities.Common;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.Enums;

namespace EStoreX.Core.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order(
        string buyerEmail,
        decimal subTotal,
        ShippingAddress shippingAddress,
        DeliveryMethod deliveryMethod,
        IEnumerable<OrderItem> orderItems,
        string paymentIntentId,
        string? discountCode = null,
        Guid? discountId = null
    )
        {
            BuyerEmail = buyerEmail;
            SubTotal = subTotal;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            PaymentIntentId = paymentIntentId;
            DiscountCode = discountCode;
            DiscountId = discountId;
        }
        public Order() { }
        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string PaymentIntentId { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public ApplicationUser Buyer { get; set; }
        public string? DiscountCode { get; set; }
        public Guid? DiscountId { get; set; }
        public decimal GetTotal()
        {
            return SubTotal + (DeliveryMethod?.Price ?? 0);
        }
    }
}
