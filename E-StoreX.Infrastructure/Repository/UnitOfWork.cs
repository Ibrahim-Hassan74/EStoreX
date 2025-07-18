using AutoMapper;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using EStoreX.Infrastructure.Data;

namespace EStoreX.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IPhotoRepository PhotoRepository { get; }

        public ICustomerBasketRepository CustomerBasketRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            CategoryRepository = new CategoryRepository(_context);
            PhotoRepository = new PhotoRepository(_context, _imageService);
            ProductRepository = new ProductRepository(_context, PhotoRepository, _imageService);
            CustomerBasketRepository = new CustomerBasketRepository();
        }
    }
}
