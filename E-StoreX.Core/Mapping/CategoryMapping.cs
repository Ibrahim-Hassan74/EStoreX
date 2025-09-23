using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.DTO.Categories.Responses;

namespace EStoreX.Core.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<UpdateCategoryDTO, Category>();
            CreateMap<Category, CategoryResponseWithPhotos>()
                .ForMember(dest => dest.Photos,
                    opt => opt.MapFrom(src =>
                        src.Photos != null ? src.Photos : new List<Photo>()
                    ));
        }
    }
}
