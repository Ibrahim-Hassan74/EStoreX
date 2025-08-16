using Domain.Entities.Common;
using System.Text.Json.Serialization;

namespace Domain.Entities.Baskets
{
    public class CustomerBasket : BaseEntity<string>
    {
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public string PaymentIntentId { get; set; } = string.Empty;

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
