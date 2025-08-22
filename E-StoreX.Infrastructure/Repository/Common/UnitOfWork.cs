using AutoMapper;
using Repository.Products;
using StackExchange.Redis;
using EStoreX.Infrastructure.Data;
using EStoreX.Core.RepositoryContracts.Account;
using EStoreX.Core.RepositoryContracts.Basket;
using EStoreX.Core.RepositoryContracts.Categories;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.RepositoryContracts.Orders;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Core.ServiceContracts.Common;
using EStoreX.Core.Repository.Categories;
using EStoreX.Core.Repository.Products;
using EStoreX.Core.Repository.Basket;
using EStoreX.Core.Repository.Orders;
using EStoreX.Core.Repository.Account;
using EStoreX.Core.RepositoryContracts.Favourites;
using EStoreX.Core.Repository.Favourites;
using EStoreX.Core.RepositoryContracts.Ratings;
using EStoreX.Infrastructure.Repository.Ratings;
using EStoreX.Infrastructure.Repository.Products;

namespace EStoreX.Core.Repository.Common
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

        public IApiClientRepository ApiClientRepository { get; }

        public IFavouriteRepository FavouriteRepository { get; }

        public IRatingRepository RatingRepository { get; }

        public IBrandRepository BrandRepository { get; }

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
            ApiClientRepository = new ApiClientRepository(_context);
            FavouriteRepository = new FavouriteRepository(_context);
            RatingRepository = new RatingRepository(_context);
            BrandRepository = new BrandRepository(_context);
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
