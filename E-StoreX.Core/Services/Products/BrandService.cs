using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Core.ServiceContracts.Common;
using EStoreX.Core.ServiceContracts.Products;
using EStoreX.Core.Services.Common;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.Services.Products
{
    /// <summary>
    /// Service implementation for managing <see cref="Brand"/> entities.
    /// </summary>
    public class BrandService : BaseService, IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IEntityImageManager<Brand> _imageManager;
        private readonly IImageService _imageService;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper, IEntityImageManager<Brand> imageManager, IImageService imageService) : base(unitOfWork, mapper)
        {
            _brandRepository = _unitOfWork.BrandRepository;
            _imageManager = imageManager;
            _imageService = imageService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _brandRepository.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<Brand?> GetBrandByIdAsync(Guid brandId)
        {
            if(brandId == Guid.Empty)
            {
                throw new ArgumentException("Brand ID cannot be empty.", nameof(brandId));
            }

            return await _brandRepository.GetByIdAsync(brandId);
        }

        /// <inheritdoc/>
        public async Task<Brand> CreateBrandAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Brand name cannot be null or empty.", nameof(name));
            }
            if (await _brandRepository.GetByNameAsync(name) != null)
            {
                throw new InvalidOperationException($"A brand with the name '{name}' already exists.");
            }

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            await _brandRepository.AddAsync(brand);
            await _unitOfWork.CompleteAsync();

            return brand;
        }

        /// <inheritdoc/>
        public async Task<Brand> UpdateBrandAsync(Guid brandId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("New name cannot be null or empty.", nameof(newName));
            }

            var existingBrand = await _brandRepository.GetByNameAsync(newName);
            if (existingBrand != null && existingBrand.Id != brandId)
            {
                throw new InvalidOperationException($"A brand with the name '{newName}' already exists.");
            }

            var brand = await _brandRepository.GetByIdAsync(brandId);
            if (brand == null)
            {
                throw new KeyNotFoundException($"Brand with ID {brandId} not found.");
            }

            brand.Name = newName;
            await _brandRepository.UpdateAsync(brand);
            await _unitOfWork.CompleteAsync();

            return brand;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteBrandAsync(Guid brandId)
        {
            if(brandId == Guid.Empty)
                throw new ArgumentException("Brand ID cannot be empty.", nameof(brandId));

            var brand = await _brandRepository.GetByIdAsync(brandId);
            if (brand == null)
                return false;

            await _brandRepository.DeleteAsync(brandId);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<Brand?> GetBrandByNameAsync(string name)
        {
            return await _brandRepository.GetByNameAsync(name);
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategoriesByBrandIdAsync(Guid brandId)
        {
            var categories = await _brandRepository.GetCategoriesByBrandIdAsync(brandId);
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }
        /// <inheritdoc/>
        public async Task<ApiResponse> GetBrandImagesAsync(Guid brandId)
        {
            return await _imageManager.GetImagesAsync(
                brandId,
                async (uow, id) => await uow.BrandRepository.GetByIdAsync(id, b => b.Photos),
                brand => brand.Photos
            );
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> DeleteBrandImageAsync(Guid brandId, Guid photoId)
        {
            return await _imageManager.DeleteImageAsync(
                brandId,
                photoId,
                async (uow, id) => await uow.BrandRepository.GetByIdAsync(id, b => b.Photos),
                brand => brand.Photos
            );
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> AddBrandImagesAsync(Guid brandId, List<IFormFile> files)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId, b => b.Photos);
            if (brand == null)
                return ApiResponseFactory.NotFound("Brand not found.");

            var folderName = brand.Name.Replace(" ", "_");

            return await _imageManager.AddImagesAsync(
                brandId,
                files,
                folderName,
                async (uow, id) => await uow.BrandRepository.GetByIdAsync(id, b => b.Photos),
                (entity, imagePaths) =>
                {
                    foreach (var path in imagePaths)
                    {
                        entity.Photos.Add(new Photo
                        {
                            BrandId = brandId,
                            ImageName = path
                        });
                    }
                }
            );
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> UpdateBrandImagesAsync(Guid brandId, List<IFormFile> files)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId, b => b.Photos);
            if (brand == null)
                return ApiResponseFactory.NotFound("Brand not found.");

            if (files == null || files.Count == 0)
                return ApiResponseFactory.BadRequest("No files provided.");

            // Delete old images
            foreach (var photo in brand.Photos.ToList())
            {
                _imageService.DeleteImageAsync(photo.ImageName);
                brand.Photos.Remove(photo);
            }

            var folderName = brand.Name.Replace(" ", "").ToLowerInvariant();

            var formFileCollection = new FormFileCollection();
            foreach (var file in files)
                formFileCollection.Add(file);

            var imagePaths = await _imageService.AddImageAsync(formFileCollection, $"Brands/{folderName}");

            foreach (var path in imagePaths)
            {
                brand.Photos.Add(new Photo
                {
                    ImageName = path,
                    BrandId = brandId
                });
            }

            await _unitOfWork.CompleteAsync();
            return ApiResponseFactory.Success("Images updated successfully.");
        }

    }
}
