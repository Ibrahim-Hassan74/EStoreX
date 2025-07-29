using AutoMapper;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using EStoreX.Infrastructure.Data;
using StackExchange.Redis;

namespace EStoreX.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IConnectionMultiplexer _redis;

        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IPhotoRepository PhotoRepository { get; }

        public ICustomerBasketRepository CustomerBasketRepository { get; }
        public IOrderRepository OrderRepository { get; }

        public IAuthenticationRepository AuthenticationRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IMapper mapper, IImageService imageService, IConnectionMultiplexer redis)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _redis = redis;
            CategoryRepository = new CategoryRepository(_context);
            PhotoRepository = new PhotoRepository(_context, _imageService);
            ProductRepository = new ProductRepository(_context, PhotoRepository, _imageService);
            CustomerBasketRepository = new CustomerBasketRepository(_redis);
            OrderRepository = new OrderRepository(_context);
            AuthenticationRepository = new AuthenticationRepository(_context);
        }
    }
}
