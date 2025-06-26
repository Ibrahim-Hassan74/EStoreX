using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;

namespace EStoreX.Core.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductRequest, Product>()
                .ForMember(x => x.Photos, opt => opt.Ignore());

            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.Photos,
                    opt => opt.MapFrom(src => src.Photos)); 


            CreateMap<Photo, PhotoResponse>();

            //CreateMap<ProductRequest, Product>()
            //    .ForMember(dest => dest.Photos,
            //               opt => opt.MapFrom(src => src.Photos));

            CreateMap<PhotoRequest, Photo>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());


        }
    }
}
