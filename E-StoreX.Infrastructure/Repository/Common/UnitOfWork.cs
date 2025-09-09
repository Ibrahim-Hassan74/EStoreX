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
using EStoreX.Core.RepositoryContracts.Favourites;
using EStoreX.Core.RepositoryContracts.Ratings;
using EStoreX.Core.RepositoryContracts.Discounts;
using EStoreX.Infrastructure.Repository.Categories;
using EStoreX.Infrastructure.Repository.Products;
using EStoreX.Infrastructure.Repository.Basket;
using EStoreX.Infrastructure.Repository.Account;
using EStoreX.Infrastructure.Repository.Favourites;
using EStoreX.Infrastructure.Repository.Ratings;
using EStoreX.Infrastructure.Repository.Orders;
using EStoreX.Infrastructure.Repositories.Discounts;

namespace EStoreX.Infrastructure.Repository.Common
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

        public IDeliveryMethodRepository DeliveryMethodRepository {  get; }

        public IDiscountRepository DiscountRepository { get; }

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
            DeliveryMethodRepository = new DeliveryMethodRepository(_context);
            DiscountRepository = new DiscountRepository(_context);

        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
