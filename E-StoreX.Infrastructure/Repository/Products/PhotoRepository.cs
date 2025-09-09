using Domain.Entities.Product;
using EStoreX.Core.DTO.Products.Responses;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Core.ServiceContracts.Common;
using EStoreX.Infrastructure.Repository.Common;

namespace EStoreX.Infrastructure.Repository.Products
{
    /// <inheritdoc/>
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        public PhotoRepository(ApplicationDbContext context, IImageService imageService) : base(context)
        {
            _context = context;
            _imageService = imageService;
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<Photo>> AddRangeAsync(PhotosDTO photosDTO)
        {
            var imagePath = await _imageService.AddImageAsync(photosDTO.FormFiles, photosDTO.Src);

            var photos = imagePath.Select(path => new Photo
            {
                ImageName = path.Replace(" ", ""),
                ProductId = photosDTO.ProductId,
                Id = Guid.NewGuid()
            }).ToList();

            await _context.AddRangeAsync(photos);
            await _context.SaveChangesAsync();

            return photos;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Photo>> UpdatePhotosAsync(PhotosDTO photos)
        {
            foreach (var photo in photos.Photos)
            {
                if (!string.IsNullOrEmpty(photo.ImageName))
                {
                    _imageService.DeleteImageAsync(photo.ImageName);
                }
            }

            _context.Photos.RemoveRange(photos.Photos);
            await _context.SaveChangesAsync();

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == photos.ProductId);

            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var rng = await AddRangeAsync(photos);


            return rng;
        }

        // You can add photo-specific methods here if needed
        // For example, methods to get photos by product, etc.
    }
}