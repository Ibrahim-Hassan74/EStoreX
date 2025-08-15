namespace EStoreX.Core.DTO.Orders.Responses
{
    public class DeliveryMethodResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; } = string.Empty;
    }
}
