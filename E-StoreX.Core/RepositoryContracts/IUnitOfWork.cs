using ServiceContracts;

namespace EStoreX.Core.RepositoryContracts
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
