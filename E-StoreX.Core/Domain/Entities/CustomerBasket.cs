using System.Text.Json.Serialization;

namespace EStoreX.Core.Domain.Entities
{
    public class CustomerBasket : BaseEntity<string>
    {
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        [JsonIgnore]
        public string PaymentIntentId { get; set; } = string.Empty;
        [JsonIgnore]
        public string ClientSecret { get; set; } = string.Empty;

        public CustomerBasket()
        {
        }
        public CustomerBasket(string id)
        {
            Id = id;
        }
    }
}
