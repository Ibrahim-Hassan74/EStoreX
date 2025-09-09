using EStoreX.Core.RepositoryContracts.Account;
using EStoreX.Core.RepositoryContracts.Basket;
using EStoreX.Core.RepositoryContracts.Categories;
using EStoreX.Core.RepositoryContracts.Discounts;
using EStoreX.Core.RepositoryContracts.Favourites;
using EStoreX.Core.RepositoryContracts.Orders;
using EStoreX.Core.RepositoryContracts.Products;
using EStoreX.Core.RepositoryContracts.Ratings;

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
        IFavouriteRepository FavouriteRepository { get; }
        IRatingRepository RatingRepository { get; }
        IBrandRepository BrandRepository { get; }
        IDeliveryMethodRepository DeliveryMethodRepository { get; }
        IDiscountRepository DiscountRepository { get; }
        Task<int> CompleteAsync();
    }
}
