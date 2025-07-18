using EStoreX.Core.Domain.Entities;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task<CustomerBasket?> GetBasketAsync(string id)
        {
            return _unitOfWork.CustomerBasketRepository.GetBasketAsync(id);
        }

        /// <inheritdoc/>
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            return _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
        }

        /// <inheritdoc/>
        public Task<bool> DeleteBasketAsync(string id)
        {
            return _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(id);
        }
    }
}
