using EStoreX.Core.Domain.Entities;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;

namespace EStoreX.Core.Services
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
