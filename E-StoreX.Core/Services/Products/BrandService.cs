using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Categories.Responses;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Core.ServiceContracts.Products;
using EStoreX.Core.Services.Common;

namespace EStoreX.Core.Services.Products
{
    /// <summary>
    /// Service implementation for managing <see cref="Brand"/> entities.
    /// </summary>
    public class BrandService : BaseService, IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _brandRepository = _unitOfWork.BrandRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _brandRepository.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<Brand?> GetBrandByIdAsync(Guid brandId)
        {
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
    }
}
