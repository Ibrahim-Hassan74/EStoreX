using Domain.Entities.Product;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Core.ServiceContracts.Products;

namespace EStoreX.Core.Services.Products
{
    /// <summary>
    /// Service implementation for managing <see cref="Brand"/> entities.
    /// </summary>
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
