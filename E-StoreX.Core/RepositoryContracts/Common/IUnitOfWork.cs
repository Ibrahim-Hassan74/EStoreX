using EStoreX.Core.RepositoryContracts.Account;
using EStoreX.Core.RepositoryContracts.Basket;
using EStoreX.Core.RepositoryContracts.Categories;
using EStoreX.Core.RepositoryContracts.Orders;
using EStoreX.Core.RepositoryContracts.Products;

namespace EStoreX.Core.RepositoryContracts.Common
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        ICustomerBasketRepository CustomerBasketRepository { get; }
        IOrderRepository OrderRepository { get; }
        IAuthenticationRepository AuthenticationRepository { get; }
        IApiClientRepository ApiClientRepository { get; }
        Task<int> CompleteAsync();
    }
}
