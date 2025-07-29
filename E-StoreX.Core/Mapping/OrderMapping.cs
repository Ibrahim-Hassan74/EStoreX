using AutoMapper;
using Domain.Entities;
using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.DTO;

namespace EStoreX.Core.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodResponse>();

            CreateMap<OrderItem, OrderItemResponse>();

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.Name))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));
            CreateMap<Address, ShippingAddressDTO>().ReverseMap();
        }
    }
}
