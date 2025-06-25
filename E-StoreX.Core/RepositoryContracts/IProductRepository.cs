using Domain.Entities.Product;
using EStoreX.Core.DTO;

namespace EStoreX.Core.RepositoryContracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(ProductRequest productRequest);
    }
}
