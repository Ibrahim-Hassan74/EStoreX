using AutoMapper;
using Domain.Entities.Baskets;
using EStoreX.Core.DTO.Basket;

namespace EStoreX.Core.Mapping
{
    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<CustomerBasket, CustomerBasketDTO>().ReverseMap();
        }
    }
}
