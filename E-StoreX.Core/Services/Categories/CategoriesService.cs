using AutoMapper;
using EStoreX.Core.Helper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.RepositoryContracts.Categories;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Categories;
using EStoreX.Core.Services.Common;

namespace EStoreX.Core.Services.Categories
{
    public class CategoriesService : BaseService, ICategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesService(IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork, mapper)
        {
            _categoryRepository = _unitOfWork.CategoryRepository;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
                throw new ArgumentNullException(nameof(categoryRequest), "Category cannot be null");

            ValidationHelper.ModelValidation(categoryRequest);

            var category = _mapper.Map<Category>(categoryRequest);

            category.Id = Guid.NewGuid(); // Ensure a new ID is generated for the category

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Category ID cannot be empty", nameof(id));

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            var res = await _categoryRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return res;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var res = categories.Select(x => _mapper.Map<CategoryResponse>(x)).ToList();
            return res;
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Category ID cannot be empty", nameof(id));
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return null;

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDto)
        {
            if(updateCategoryDto is null)
                throw new ArgumentNullException(nameof(updateCategoryDto), "Category cannot be null");

            ValidationHelper.ModelValidation(updateCategoryDto);

            var category = await _categoryRepository.GetByIdAsync(updateCategoryDto.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {updateCategoryDto.Id} not found.");

            _mapper.Map(updateCategoryDto, category);

            var res = await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryResponse>(category);
        }
        public async Task<IEnumerable<Brand>> GetBrandsByCategoryIdAsync(Guid categoryId)
        {
            var brands = await _categoryRepository.GetBrandsByCategoryIdAsync(categoryId);
            return brands;
        }

        public async Task<bool> AssignBrandToCategoryAsync(CategoryBrand cb)
        {
            return await _categoryRepository.AssignBrandAsync(cb);
        }

        public async Task<bool> UnassignBrandFromCategoryAsync(CategoryBrand cb)
        {
            return await _categoryRepository.UnassignBrandAsync(cb);
        }
    }
}
