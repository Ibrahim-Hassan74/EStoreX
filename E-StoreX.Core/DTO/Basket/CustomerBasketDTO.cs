using Domain.Entities.Baskets;
using Domain.Entities.Common;

namespace EStoreX.Core.DTO.Basket
{
    public class CustomerBasketDTO : BaseEntity<string>
    {
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public CustomerBasketDTO()
        {

        }
        public CustomerBasketDTO(string id)
        {
            Id = id;
        }
    }
    public class BasketAddRequest
    {
        public BasketItem BasketItem { get; set; }
        public string BasketId { get; set; } = string.Empty;
    }
}
