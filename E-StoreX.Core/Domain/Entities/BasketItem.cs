namespace EStoreX.Core.Domain.Entities
{
    public class BasketItem : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public int Qunatity { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

    }
}