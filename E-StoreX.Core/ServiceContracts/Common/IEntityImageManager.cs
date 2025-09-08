using Domain.Entities.Product;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.RepositoryContracts.Common;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.ServiceContracts.Common
{
    /// <summary>
    /// Provides a generic contract for managing entity-related images,
    /// including CRUD operations on photos associated with entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that contains image references.</typeparam>
    public interface IEntityImageManager<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves all images associated with a specific entity.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity.</param>
        /// <param name="getEntityFunc">
        /// A function to retrieve the entity from the database, including its images.
        /// </param>
        /// <param name="getImages">
        /// A function to extract the images collection from the entity.
        /// </param>
        /// <returns>
        /// <see cref="ApiResponse"/> containing the list of images if found;
        /// <c>404 Not Found</c> if the entity does not exist.
        /// </returns>
        Task<ApiResponse> GetImagesAsync(
            Guid entityId,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Func<TEntity, IEnumerable<Photo>> getImages
        );

        /// <summary>
        /// Adds new images to the specified entity without removing existing ones.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity.</param>
        /// <param name="files">The list of image files to upload.</param>
        /// <param name="folderName">The folder name where the images should be stored.</param>
        /// <param name="getEntityFunc">
        /// A function to retrieve the entity from the database.
        /// </param>
        /// <param name="assignImages">
        /// An action to assign the uploaded image paths to the entity's image collection.
        /// </param>
        /// <returns>
        /// <see cref="ApiResponse"/> indicating the result of the add operation;
        /// <c>400 Bad Request</c> if no files are provided;
        /// <c>404 Not Found</c> if the entity does not exist.
        /// </returns>
        Task<ApiResponse> AddImagesAsync(
            Guid entityId,
            List<IFormFile> files,
            string folderName,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Action<TEntity, List<string>> assignImages
        );

        /// <summary>
        /// Replaces all existing images of the specified entity with new ones.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity.</param>
        /// <param name="files">The list of new image files to upload.</param>
        /// <param name="folderName">The folder name where the images should be stored.</param>
        /// <param name="getEntityFunc">
        /// A function to retrieve the entity from the database.
        /// </param>
        /// <param name="getImages">
        /// A function to extract the current images collection from the entity.
        /// </param>
        /// <param name="assignImages">
        /// An action to assign the newly uploaded image paths to the entity's image collection.
        /// </param>
        /// <returns>
        /// <see cref="ApiResponse"/> indicating the result of the update operation;
        /// <c>400 Bad Request</c> if no files are provided;
        /// <c>404 Not Found</c> if the entity does not exist.
        /// </returns>
        Task<ApiResponse> UpdateImagesAsync(
            Guid entityId,
            List<IFormFile> files,
            string folderName,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Func<TEntity, ICollection<Photo>> getImages,
            Action<TEntity, List<string>> assignImages
        );

        /// <summary>
        /// Deletes a specific image from the specified entity.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity.</param>
        /// <param name="photoId">The unique identifier of the photo to delete.</param>
        /// <param name="getEntityFunc">
        /// A function to retrieve the entity from the database.
        /// </param>
        /// <param name="getImages">
        /// A function to extract the images collection from the entity.
        /// </param>
        /// <returns>
        /// <see cref="ApiResponse"/> indicating the result of the delete operation;
        /// <c>404 Not Found</c> if the entity or photo does not exist.
        /// </returns>
        Task<ApiResponse> DeleteImageAsync(
            Guid entityId,
            Guid photoId,
            Func<IUnitOfWork, Guid, Task<TEntity>> getEntityFunc,
            Func<TEntity, ICollection<Photo>> getImages
        );
    }
}
