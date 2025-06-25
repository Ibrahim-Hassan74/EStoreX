using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using EStoreX.Infrastructure.Data;

namespace EStoreX.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public ProductRepository(ApplicationDbContext context, IMapper mapper, IImageService imageService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<Product> AddProductAsync(ProductRequest productRequest)
        {
            var product = _mapper.Map<Product>(productRequest);
            product.Id = Guid.NewGuid();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var imagePath = await _imageService.AddImageAsync(productRequest.Photos, productRequest.Name);

            var photos = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,
                Id = Guid.NewGuid()
            }).ToList();

            await _context.AddRangeAsync(photos);
            await _context.SaveChangesAsync();

            return product;
        }
        // You can add product-specific methods here if needed
        // For example, methods to get products by category, price range, etc.
    }
}
