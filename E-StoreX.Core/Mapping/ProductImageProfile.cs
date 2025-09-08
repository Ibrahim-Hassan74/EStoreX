using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Products.Responses;

namespace EStoreX.Core.Mapping
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<Photo, ProductImageResponse>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImageName));
        }
    }
}
