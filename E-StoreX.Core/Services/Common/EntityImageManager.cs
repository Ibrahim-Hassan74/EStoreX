using Domain.Entities.Product;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Products.Responses;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Common;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.Services.Common
{
    public class EntityImageManager<TEntity> : IEntityImageManager<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public EntityImageManager(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        /// <inheritdoc/>
        public async Task<ApiResponse> GetImagesAsync(
            Guid entityId,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Func<TEntity, IEnumerable<Photo>> getImages)
        {
            var entity = await getEntityFunc(_unitOfWork, entityId);
            if (entity == null)
                return ApiResponseFactory.NotFound($"{typeof(TEntity).Name} not found.");

            var images = getImages(entity).Select(p => new PhotoInfo() { ImageName = p.ImageName, Id = p.Id }).ToList();
            return ApiResponseFactory.Success("Images retrieved successfully.", images);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> AddImagesAsync(
            Guid entityId,
            List<IFormFile> files,
            string folderName,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Action<TEntity, List<string>> assignImages)
        {
            var entity = await getEntityFunc(_unitOfWork, entityId);
            if (entity == null)
                return ApiResponseFactory.NotFound($"{typeof(TEntity).Name} not found.");
            if (files == null || files.Count == 0)
                return ApiResponseFactory.BadRequest("No files provided.");

            var formFiles = new FormFileCollection();
            foreach (var f in files) formFiles.Add(f);

            var imagePaths = await _imageService.AddImageAsync(formFiles, $"{folderName}");
            assignImages(entity, imagePaths);

            await _unitOfWork.CompleteAsync();
            return ApiResponseFactory.Success("Images added successfully.");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> UpdateImagesAsync(
            Guid entityId,
            List<IFormFile> files,
            string folderName,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Func<TEntity, ICollection<Photo>> getImages,
            Action<TEntity, List<string>> assignImages)
        {
            var entity = await getEntityFunc(_unitOfWork, entityId);
            if (entity == null)
                return ApiResponseFactory.NotFound($"{typeof(TEntity).Name} not found.");
            if (files == null || files.Count == 0)
                return ApiResponseFactory.BadRequest("No files provided.");

            // delete old
            foreach (var photo in getImages(entity).ToList())
            {
                _imageService.DeleteImageAsync(photo.ImageName);
                getImages(entity).Remove(photo);
            }

            // add new
            var formFiles = new FormFileCollection();
            foreach (var f in files) formFiles.Add(f);

            var imagePaths = await _imageService.AddImageAsync(formFiles, $"{folderName}");
            assignImages(entity, imagePaths);

            await _unitOfWork.CompleteAsync();
            return ApiResponseFactory.Success("Images updated successfully.");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse> DeleteImageAsync(
            Guid entityId,
            Guid photoId,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Func<TEntity, ICollection<Photo>> getImages)
        {
            var entity = await getEntityFunc(_unitOfWork, entityId);
            if (entity == null)
                return ApiResponseFactory.NotFound($"{typeof(TEntity).Name} not found.");

            var photo = getImages(entity).FirstOrDefault(p => p.Id == photoId);
            if (photo == null)
                return ApiResponseFactory.NotFound("Photo not found.");

            _imageService.DeleteImageAsync(photo.ImageName);
            getImages(entity).Remove(photo);

            await _unitOfWork.CompleteAsync();
            return ApiResponseFactory.Success("Image deleted successfully.");
        }
    }
}
