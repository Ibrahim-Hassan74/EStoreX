using EStoreX.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace EStoreX.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IFileProvider _fileProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IFileProvider fileProvider, IWebHostEnvironment webHostEnvironment)
        {
            _fileProvider = fileProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var saveImageSrc = new List<string>();
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("No files provided for image upload.", nameof(files));
            }
            if (string.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentException("Source folder cannot be null or empty.", nameof(src));
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", src);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var imagePath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    saveImageSrc.Add(Path.Combine("Images", src, fileName));
                }
            }
            return saveImageSrc;
        }

        public bool DeleteImageAsync(string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentException("Source path cannot be null or empty.", nameof(src));
            }

            var info = _fileProvider.GetFileInfo(src);
            if (!info.Exists)
            {
                throw new FileNotFoundException("The specified image does not exist.", src);
            }

            var root = info.PhysicalPath;
            if (!File.Exists(root))
            {
                return false;
            }
            File.Delete(root);

            return true;
        }
    }
}
