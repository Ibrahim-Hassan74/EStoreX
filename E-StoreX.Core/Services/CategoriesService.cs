using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;

namespace EStoreX.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoriesService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto), "Category cannot be null");
            VaildationHelper.ModelValidation(categoryDto);
            var category = _mapper.Map<Category>(categoryDto);

            category.Id = Guid.NewGuid(); // Ensure a new ID is generated for the category

            await _categoryRepository.AddAsync(category);

            return _mapper.Map<CategoryDTO>(category);
        }

        public Task<bool> DeleteCategoryAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDTO?> GetCategoryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDTO> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
