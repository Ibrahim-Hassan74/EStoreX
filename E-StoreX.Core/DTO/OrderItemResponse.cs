namespace EStoreX.Core.DTO
{
    public class OrderItemResponse
    {
        public Guid ProductItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string MainImage { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
