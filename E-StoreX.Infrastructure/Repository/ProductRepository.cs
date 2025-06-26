using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using EStoreX.Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoRepository _photosRepository;
        public ProductRepository(ApplicationDbContext context, IPhotoRepository photosRepository) : base(context)
        {
            _context = context;
            _photosRepository = photosRepository;
        }

        public async Task<Product> AddProductAsync(Product product, IFormFileCollection formFiles)
        {

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var photosDTO = new PhotosDTO()
            {
                ProductId = product.Id,
                Src = product.Name,
                formFiles = formFiles
            };
            var photos = await _photosRepository.AddRangeAsync(photosDTO);

            #region Commented out code for image handling
            //var imagePath = await _imageService.AddImageAsync(productRequest.Photos, productRequest.Name);

            //var photos = imagePath.Select(path => new Photo
            //{
            //    ImageName = path,
            //    ProductId = product.Id,
            //    Id = Guid.NewGuid()
            //}).ToList();

            //await _context.AddRangeAsync(photos);
            //await _context.SaveChangesAsync();
            #endregion

            return product;
        }
        // You can add product-specific methods here if needed
        // For example, methods to get products by category, price range, etc.
    }
}
