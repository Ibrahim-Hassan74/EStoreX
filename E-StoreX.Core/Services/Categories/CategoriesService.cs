﻿using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Requests;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts.Categories;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Categories;
using EStoreX.Core.ServiceContracts.Common;
using EStoreX.Core.Services.Common;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.Services.Categories
{
    public class CategoriesService : BaseService, ICategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEntityImageManager<Category> _imageManager;
        private readonly IImageService _imageService;
        public CategoriesService(IMapper mapper, IUnitOfWork unitOfWork, IEntityImageManager<Category> imageManager, IImageService imageService) : base(unitOfWork, mapper)
        {
            _categoryRepository = _unitOfWork.CategoryRepository;
            _imageManager = imageManager;
            _imageService = imageService;
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
        /// <inheritdoc/>
        public async Task<ApiResponse> GetCategoryImagesAsync(Guid categoryId)
        {
            return await _imageManager.GetImagesAsync(
                categoryId,
                async (uow, id) => await uow.CategoryRepository.GetByIdAsync(id, c => c.Photos),
                category => category.Photos
            );
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> DeleteCategoryImageAsync(Guid categoryId, Guid photoId)
        {
            return await _imageManager.DeleteImageAsync(
                categoryId,
                photoId,
                async (uow, id) => await uow.CategoryRepository.GetByIdAsync(id, c => c.Photos),
                category => category.Photos
            );
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> AddCategoryImagesAsync(Guid categoryId, List<IFormFile> files)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId, c => c.Photos);
            if (category == null)
                return ApiResponseFactory.NotFound("Category not found.");

            var folderName = category.Name.Replace(" ", "");

            return await _imageManager.AddImagesAsync(
                categoryId,
                files,
                folderName,
                async (uow, id) => await uow.CategoryRepository.GetByIdAsync(id, c => c.Photos),
                (entity, imagePaths) =>
                {
                    foreach (var path in imagePaths)
                    {
                        entity.Photos.Add(new Photo
                        {
                            CategoryId = categoryId,
                            ImageName = path
                        });
                    }
                }
            );
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> UpdateCategoryImagesAsync(Guid categoryId, List<IFormFile> files)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId, c => c.Photos);
            if (category == null)
                return ApiResponseFactory.NotFound("Category not found.");

            if (files == null || files.Count == 0)
                return ApiResponseFactory.BadRequest("No files provided.");

            // Delete old images
            foreach (var photo in category.Photos.ToList())
            {
                _imageService.DeleteImageAsync(photo.ImageName);
                category.Photos.Remove(photo);
            }

            var folderName = category.Name.Replace(" ", "").ToLowerInvariant();

            var formFileCollection = new FormFileCollection();
            foreach (var file in files)
                formFileCollection.Add(file);

            var imagePaths = await _imageService.AddImageAsync(formFileCollection, $"Categories/{folderName}");

            foreach (var path in imagePaths)
            {
                category.Photos.Add(new Photo
                {
                    ImageName = path,
                    CategoryId = categoryId
                });
            }

            await _unitOfWork.CompleteAsync();
            return ApiResponseFactory.Success("Images updated successfully.");
        }

    }
}
