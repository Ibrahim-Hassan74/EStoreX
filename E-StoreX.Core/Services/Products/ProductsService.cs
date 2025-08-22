using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Products.Requests;
using EStoreX.Core.DTO.Products.Responses;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Core.ServiceContracts.Products;
using EStoreX.Core.Services.Common;

namespace EStoreX.Core.Services.Products
{
    public class ProductsService : BaseService, IProductsService
    {
        private readonly IProductRepository _productRepository;
        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _productRepository = unitOfWork.ProductRepository;
        }

        public async Task<ProductResponse> CreateProductAsync(ProductAddRequest productRequest)
        {
            if (productRequest == null)
            {
                throw new ArgumentNullException(nameof(productRequest), "Product request cannot be null.");
            }
            ValidationHelper.ModelValidation(productRequest);

            var product = _mapper.Map<Product>(productRequest);
            product.Id = Guid.NewGuid();

            product = await _productRepository.AddProductAsync(product, productRequest.Photos);

            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id, x => x.Category, y => y.Photos);

            if (product == null)
                return false;

            return await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync(x => x.Category, y => y.Photos, b => b.Brand);
            var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productResponses;
        }

        public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(id));
            }

            var product = await _productRepository.GetByIdAsync(id, x => x.Category, y => y.Photos, b => b.Brand);

            if (product == null)
                return null;

            var productResponse = _mapper.Map<ProductResponse>(product);
            return productResponse;
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest productUpdateRequest)
        {
            if (productUpdateRequest == null)
            {
                throw new ArgumentNullException(paramName: nameof(productUpdateRequest), "Product update request cannot be null.");
            }

            ValidationHelper.ModelValidation(productUpdateRequest);

            var findProduct = await _productRepository.GetByIdAsync(productUpdateRequest.Id, x => x.Category, x => x.Photos, b => b.Brand);

            if (findProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productUpdateRequest.Id} not found.");
            }

            findProduct.Id = productUpdateRequest.Id;
            findProduct.Name = productUpdateRequest.Name;
            findProduct.Description = productUpdateRequest.Description;
            findProduct.OldPrice = productUpdateRequest.OldPrice;
            findProduct.NewPrice = productUpdateRequest.NewPrice;
            findProduct.CategoryId = productUpdateRequest.CategoryId;
            findProduct.BrandId = productUpdateRequest.BrandId;

            var productResponse = await _productRepository.UpdateProductAsync(findProduct, productUpdateRequest.Photos);

            return _mapper.Map<ProductResponse>(findProduct);
        }

        public async Task<IEnumerable<ProductResponse>> GetFilteredProductsAsync(ProductQueryDTO query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), "Query cannot be null.");
            }

            var products = await _productRepository.GetFilteredProductsAsync(query);
            return _mapper.Map<IEnumerable<ProductResponse>>(products);
        }

        public async Task<int> CountProductsAsync()
        {
            return await _productRepository.CountAsync();
        }
    }
}
