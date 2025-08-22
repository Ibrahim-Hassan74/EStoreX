using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Products.Requests;
using EStoreX.Core.DTO.Products.Responses;

namespace EStoreX.Core.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductAddRequest, Product>()
                .ForMember(x => x.Photos, opt => opt.Ignore());

            CreateMap<ProductUpdateRequest, Product>()
                .ForMember(x => x.Photos, opt => opt.Ignore());

            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.Photos,
                    opt => opt.MapFrom(src => src.Photos))
                .ForMember(dest => dest.QuantityAvailable, opt => opt.MapFrom(src => src.QuantityAvailable))
                 .ForMember(dest => dest.BrandName, 
                 opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : string.Empty));


            CreateMap<Photo, PhotoResponse>();

            //CreateMap<ProductRequest, Product>()
            //    .ForMember(dest => dest.Photos,
            //               opt => opt.MapFrom(src => src.Photos));

            CreateMap<PhotoRequest, Photo>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());


        }
    }
}
