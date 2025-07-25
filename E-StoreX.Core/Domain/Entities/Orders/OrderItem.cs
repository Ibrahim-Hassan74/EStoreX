namespace EStoreX.Core.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem(decimal price, int quantity, Guid productItemId, string mainImage, string productName)
        {
            Price = price;
            Quantity = quantity;
            ProductItemId = productItemId;
            MainImage = mainImage;
            ProductName = productName;
        }
        public OrderItem() { }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid ProductItemId { get; set; }
        public string MainImage { get; set; }
        public string ProductName { get; set; }

    }
}