using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;

namespace EStoreX.Core.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<UpdateCategoryDTO, Category>();
        }
    }
}
