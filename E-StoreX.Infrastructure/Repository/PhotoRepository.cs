using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using EStoreX.Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Infrastructure.Repository
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        public PhotoRepository(ApplicationDbContext context, IImageService imageService) : base(context)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IEnumerable<Photo>> AddRangeAsync(PhotosDTO photosDTO)
        {
            var imagePath = await _imageService.AddImageAsync(photosDTO.formFiles, photosDTO.Src);

            var photos = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = photosDTO.ProductId,
                Id = Guid.NewGuid()
            }).ToList();

            await _context.AddRangeAsync(photos);
            await _context.SaveChangesAsync();

            return photos;
        }
        // You can add photo-specific methods here if needed
        // For example, methods to get photos by product, etc.
    }
}