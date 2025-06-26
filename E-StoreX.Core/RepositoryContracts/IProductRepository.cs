using Domain.Entities.Product;
using EStoreX.Core.DTO;
using Microsoft.AspNetCore.Http;

namespace EStoreX.Core.RepositoryContracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> AddProductAsync(Product product, IFormFileCollection formFiles);
    }
}
