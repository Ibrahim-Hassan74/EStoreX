using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;

namespace EStoreX.Core.Services
{
    public class CategoriesService : BaseService, ICategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesService(IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork, mapper)
        {
            _categoryRepository = _unitOfWork.CategoryRepository;
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

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Category ID cannot be empty", nameof(id));
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            var res = await _categoryRepository.DeleteAsync(id);

            return res;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var res = categories.Select(x => _mapper.Map<CategoryDTO>(x)).ToList();
            return res;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Category ID cannot be empty", nameof(id));
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return null;

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDto)
        {
            if(updateCategoryDto is null)
                throw new ArgumentNullException(nameof(updateCategoryDto), "Category cannot be null");

            VaildationHelper.ModelValidation(updateCategoryDto);

            var category = await _categoryRepository.GetByIdAsync(updateCategoryDto.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {updateCategoryDto.Id} not found.");

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;

            var res = await _categoryRepository.UpdateAsync(category);

            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
