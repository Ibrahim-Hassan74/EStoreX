using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;

namespace EStoreX.Core.Services
{
    public class ProductsService : BaseService, IProductsService
    {
        private readonly IProductRepository _productRepository;
        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _productRepository = unitOfWork.ProductRepository;
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest productRequest)
        {
            if (productRequest == null)
            {
                throw new ArgumentNullException(nameof(productRequest), "Product request cannot be null.");
            }
            ValidationHelper.ModelValidation(productRequest);

            var product = await _productRepository.AddProductAsync(productRequest);

            return _mapper.Map<ProductResponse>(product);
        }

        public Task DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync(x => x.Category, y => y.Photos);
            var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productResponses;
        }

        public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(id));
            }

            var product = await _productRepository.GetByIdAsync(id, x => x.Category, y => y.Photos);
            
            if (product == null)
                return null;

            var productResponse = _mapper.Map<ProductResponse>(product);
            return productResponse;
        }

        public Task<ProductResponse> UpdateProductAsync(Guid id, ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }
    }
}
