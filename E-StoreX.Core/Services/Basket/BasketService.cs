using AutoMapper;
using Domain.Entities.Baskets;
using EStoreX.Core.DTO.Basket;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Basket;
using EStoreX.Core.Services.Common;

namespace EStoreX.Core.Services.Basket
{
    /// <summary>
    /// Service implementation for managing customer baskets.
    /// </summary>
    public class BasketService : BaseService, IBasketService
    {

        public BasketService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <inheritdoc/>
        public async Task<CustomerBasketDTO?> GetBasketAsync(string id)
        {
            var basketResponse = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(id);
            return _mapper.Map<CustomerBasketDTO>(basketResponse);
        }

        /// <inheritdoc/>
        public async Task<CustomerBasketDTO?> UpdateBasketAsync(CustomerBasketDTO basket)
        {
            var basketItems = new List<BasketItem>();

            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                if (product is null) continue;
                if(product.QuantityAvailable < item.Qunatity) continue;
                if(item.Qunatity <= 0) continue;

                basketItems.Add(new BasketItem()
                {
                    Id = item.Id,
                    Name = product.Name,
                    Price = product.NewPrice,
                    Description = product.Description,
                    Qunatity = item.Qunatity,
                    Category = item.Category,
                    Image = item.Image,
                });
            }

            if (basketItems.Count == 0)
            {
                return null;
            }

            var existingBasket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basket.Id);

            if (existingBasket != null)
            {
                foreach (var newItem in basketItems)
                {
                    var existingItem = existingBasket.BasketItems.FirstOrDefault(x => x.Id == newItem.Id);
                    if (existingItem != null)
                    {
                        existingItem.Qunatity += newItem.Qunatity;
                        existingItem.Price = newItem.Price; 
                    }
                    else
                    {
                        existingBasket.BasketItems.Add(newItem);
                    }
                }

                var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(existingBasket);
                return _mapper.Map<CustomerBasketDTO>(updatedBasket);
            }
            else
            {
                var newBasket = new CustomerBasket(basket.Id)
                {
                    BasketItems = basketItems,
                };

                var basketResponse = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(newBasket);
                return _mapper.Map<CustomerBasketDTO>(basketResponse);
            }
        }

        //public async Task<CustomerBasketDTO?> UpdateBasketAsync(CustomerBasketDTO basket)
        //{
        //    var basketItems = new List<BasketItem>();
        //    foreach (var item in basket.BasketItems)
        //    {
        //        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
        //        if (product is null) continue;
        //        basketItems.Add(new BasketItem()
        //        {
        //            Id = item.Id,
        //            Name = product.Name,
        //            Price = product.NewPrice,
        //            Description = product.Description,
        //            Qunatity = item.Qunatity,
        //            Category = item.Category,
        //            Image = item.Image,
        //        });
        //    }
        //    if (basketItems.Count == 0)
        //    {
        //        return null;
        //    }
        //    var newBasket = new CustomerBasket(basket.Id)
        //    {
        //        BasketItems = basketItems,
        //    };
        //    var basketResponse = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(newBasket);
        //    return _mapper.Map<CustomerBasketDTO>(basketResponse);
        //}

        /// <inheritdoc/>
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(id);
        }

        /// <inheritdoc/>
        public async Task<CustomerBasketDTO?> MergeBasketsAsync(string guestId, string userId)
        {
            var guestBasket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(guestId);
            var userBasket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(userId);

            if (guestBasket == null && userBasket == null) return null;
            if (guestBasket != null && userBasket == null)
            {
                await _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(guestId);
                guestBasket.Id = userId;
                CustomerBasket? basketResponse = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(guestBasket);
                return _mapper.Map<CustomerBasketDTO>(basketResponse);
            }
            if (guestBasket == null && userBasket != null)
            {
                return _mapper.Map<CustomerBasketDTO>(userBasket);
            }

            foreach (var item in guestBasket.BasketItems)
            {
                var existing = userBasket.BasketItems.FirstOrDefault(x => x.Id == item.Id);
                if (existing != null)
                {
                    existing.Qunatity += item.Qunatity;
                }
                else
                {
                    userBasket.BasketItems.Add(item);
                }
            }

            await _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(guestId);
            var basket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(userBasket);
            return _mapper.Map<CustomerBasketDTO>(basket);
        }

        /// <inheritdoc/>
        public async Task<CustomerBasketDTO?> DecreaseItemQuantityAsync(string basketId, Guid productId)
        {
            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            var item = basket.BasketItems.FirstOrDefault(x => x.Id == productId);
            if (item == null) return null;

            item.Qunatity--;

            if (item.Qunatity <= 0)
            {
                basket.BasketItems.Remove(item);
            }

            var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
            return _mapper.Map<CustomerBasketDTO>(updatedBasket);
        }

        /// <inheritdoc/>
        public async Task<CustomerBasketDTO?> RemoveItemAsync(string basketId, Guid productId)
        {
            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            var item = basket.BasketItems.FirstOrDefault(x => x.Id == productId);
            if (item == null) return null;

            basket.BasketItems.Remove(item);

            var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
            return _mapper.Map<CustomerBasketDTO>(updatedBasket);
        }
    }
}
