using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using EStoreX.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoRepository _photosRepository;
        private readonly IImageService _imageService;
        public ProductRepository(ApplicationDbContext context, IPhotoRepository photosRepository, IImageService imageService) : base(context)
        {
            _context = context;
            _photosRepository = photosRepository;
            _imageService = imageService;
        }

        public async Task<Product> AddProductAsync(Product product, IFormFileCollection formFiles)
        {

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var photosDTO = new PhotosDTO()
            {
                ProductId = product.Id,
                Src = product.Name,
                FormFiles = formFiles
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

        public async Task<bool> DeleteAsync(Product product)
        {
            var photos = await _context.Photos.Where(x => x.ProductId == product.Id).ToListAsync();

            foreach (var photo in photos)
            {
                if (!string.IsNullOrEmpty(photo.ImageName))
                    _imageService.DeleteImageAsync(photo.ImageName);
            }

            _context.Products.Remove(product);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<Product> UpdateProductAsync(Product product, IFormFileCollection formFiles)
        {
            var res = await UpdateAsync(product);

            PhotosDTO photosDTO = new PhotosDTO()
            {
                ProductId = product.Id,
                Src = product.Name,
                FormFiles = formFiles,
                Photos = product.Photos
            };

            if (photosDTO is not null)
            {
                res.Photos = (await _photosRepository.UpdatePhotosAsync(photosDTO)).ToList();
            }


            return res;
        }
        // You can add product-specific methods here if needed
        // For example, methods to get products by category, price range, etc.
    }
}
