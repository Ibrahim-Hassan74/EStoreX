using Domain.Entities.Baskets;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Basket;

namespace EStoreX.Core.Services.Basket
{
    /// <summary>
    /// Service implementation for managing customer baskets.
    /// </summary>
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            return await _unitOfWork.CustomerBasketRepository.GetBasketAsync(id);
        }

        /// <inheritdoc/>
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            return await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(id);
        }
    }
}
