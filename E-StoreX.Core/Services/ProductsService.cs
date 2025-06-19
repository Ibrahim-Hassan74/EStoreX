using AutoMapper;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;

namespace EStoreX.Core.Services
{
    public class ProductsService : BaseService, IProductsService
    {
        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public Task<ProductResponse> CreateProductAsync(ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(x => x.Category, y => y.Photos);
            var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productResponses;
        }

        public Task<ProductResponse> GetProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponse> UpdateProductAsync(Guid id, ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }
    }
}
