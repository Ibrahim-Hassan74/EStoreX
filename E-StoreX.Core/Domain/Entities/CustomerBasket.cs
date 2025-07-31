namespace EStoreX.Core.Domain.Entities
{
    public class CustomerBasket : BaseEntity<string>
    {
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public CustomerBasket()
        {
        }
        public CustomerBasket(string id)
        {
            Id = id;
        }
    }
}
