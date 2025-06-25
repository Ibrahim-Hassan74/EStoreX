using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.ServiceContracts
{
    public interface IImageService
    {
        /// <summary>
        /// Adds images to a specified folder and returns the list of image paths.
        /// </summary>
        /// <param name="files">collection of files</param>
        /// <param name="src">folder name</param>
        /// <returns>the src for every image that has been added</returns>
        Task<List<string>> AddImageAsync(IFormFileCollection files, string src);
        /// <summary>
        /// Deletes an image from the specified folder based on the provided source path.
        /// </summary>
        /// <param name="src">the image path</param>
        /// <returns>true / false</returns>
        bool DeleteImageAsync(string src);
    }
}
