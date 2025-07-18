using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO;
using EStoreX.Core.Enums;
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

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductQueryDTO query)
        {
            IQueryable<Product> products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Photos);

            products = ApplyFiltering(products, query);
            products = ApplySorting(products, query);
            products = ApplyPagination(products, query);

            return await products.ToListAsync();
        }


        private IQueryable<Product> ApplyFiltering(IQueryable<Product> products, ProductQueryDTO query)
        {
            if (!string.IsNullOrWhiteSpace(query.SearchBy) && !string.IsNullOrWhiteSpace(query.SearchString))
            {
                var words = query.SearchString.Split(' ');
                switch (query.SearchBy.ToLower())
                {
                    case "name":
                    case "description":
                        products = products.Where(p => words.All(w => p.Name.ToLower().Contains(w.ToLower()) 
                        || p.Description.ToLower().Contains(w.ToLower())));
                        break;
                    case "category":
                        products = products.Where(p => p.Category.Name.Contains(query.SearchString));
                        break;
                }
            }

            if (query.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == query.CategoryId.Value);
            }

            if (query.MinPrice.HasValue)
            {
                products = products.Where(p => p.NewPrice >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p => p.NewPrice <= query.MaxPrice.Value);
            }

            return products;
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> products, ProductQueryDTO query)
        {
            var sortBy = query.SortBy ?? nameof(Product.NewPrice);
            bool isAscending = query.SortOrder == SortOrderOptions.ASC;

            switch (sortBy)
            {
                case var s when s == nameof(Product.Name):
                    products = isAscending
                        ? products.OrderBy(p => p.Name)
                        : products.OrderByDescending(p => p.Name);
                    break;

                case var s when s == nameof(Product.NewPrice):
                    products = isAscending
                        ? products.OrderBy(p => p.NewPrice)
                        : products.OrderByDescending(p => p.NewPrice);
                    break;

                case var s when s == nameof(Product.OldPrice):
                    products = isAscending
                        ? products.OrderBy(p => p.OldPrice)
                        : products.OrderByDescending(p => p.OldPrice);
                    break;

                case "Category":
                    products = isAscending
                        ? products.OrderBy(p => p.Category.Name)
                        : products.OrderByDescending(p => p.Category.Name);
                    break;

                default:
                    products = isAscending
                        ? products.OrderBy(p => p.NewPrice)
                        : products.OrderByDescending(p => p.NewPrice);
                    break;
            }

            return products;
        }

        private IQueryable<Product> ApplyPagination(IQueryable<Product> products, ProductQueryDTO query)
        {
            int pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            int pageSize = query.PageSize < 1 ? 10 : query.PageSize;

            int skipAmount = (pageNumber - 1) * pageSize;

            return products.Skip(skipAmount).Take(pageSize);
        }


        // You can add product-specific methods here if needed
        // For example, methods to get products by category, price range, etc.
    }
}
