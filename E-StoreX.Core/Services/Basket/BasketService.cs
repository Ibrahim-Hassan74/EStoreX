﻿using AutoMapper;
using Domain.Entities.Baskets;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Basket;
using EStoreX.Core.Enums;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Basket;
using EStoreX.Core.Services.Common;
using Org.BouncyCastle.Bcpg;

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

        #region Old AddItemToBasketAsync
        /// <inheritdoc/>
        //public async Task<CustomerBasketDTO?> AddItemToBasketAsync(CustomerBasketDTO basket)
        //{
        //    var basketItems = new List<BasketItem>();

        //    foreach (var item in basket.BasketItems)
        //    {
        //        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
        //        if (product is null) continue;
        //        if(product.QuantityAvailable < item.Qunatity) continue;
        //        if(item.Qunatity <= 0) continue;

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

        //    var existingBasket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basket.Id);

        //    if (existingBasket != null)
        //    {
        //        foreach (var newItem in basketItems)
        //        {
        //            var existingItem = existingBasket.BasketItems.FirstOrDefault(x => x.Id == newItem.Id);
        //            if (existingItem != null)
        //            {
        //                existingItem.Qunatity += newItem.Qunatity;
        //                existingItem.Price = newItem.Price; 
        //            }
        //            else
        //            {
        //                existingBasket.BasketItems.Add(newItem);
        //            }
        //        }

        //        var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(existingBasket);
        //        return _mapper.Map<CustomerBasketDTO>(updatedBasket);
        //    }
        //    else
        //    {
        //        var newBasket = new CustomerBasket(basket.Id)
        //        {
        //            BasketItems = basketItems,
        //        };

        //        var basketResponse = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(newBasket);
        //        return _mapper.Map<CustomerBasketDTO>(basketResponse);
        //    }
        //}
        #endregion

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

            basket.Total = basket.BasketItems.Sum(x => x.Price * x.Qunatity);

            var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
            if (basket.DiscountId.HasValue && !string.IsNullOrEmpty(basket.DiscountCode))
            {
                var res = await ApplyDiscountAsync(basket.Id, basket.DiscountCode);
                if (res != null) return res;
            }
            return _mapper.Map<CustomerBasketDTO>(updatedBasket);
        }

        /// <inheritdoc/>
        public async Task<CustomerBasketDTO?> IncreaseItemQuantityAsync(string basketId, Guid productId)
        {
            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            var item = basket.BasketItems.FirstOrDefault(x => x.Id == productId);
            if (item == null) return null;

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null) return null;

            if (item.Qunatity < product.QuantityAvailable)
            {
                item.Qunatity++;
            }
            else
            {
                return _mapper.Map<CustomerBasketDTO>(basket);
            }

            basket.Total = basket.BasketItems.Sum(x => x.Price * x.Qunatity);
            var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);

            if (basket.DiscountId.HasValue && !string.IsNullOrEmpty(basket.DiscountCode))
            {
                var res = await ApplyDiscountAsync(basket.Id, basket.DiscountCode);
                if (res != null) return res;
            }
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

            basket.Total = basket.BasketItems.Sum(x => x.Price * x.Qunatity);

            var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
            if (basket.DiscountId.HasValue && !string.IsNullOrEmpty(basket.DiscountCode))
            {
                var res = await ApplyDiscountAsync(basket.Id, basket.DiscountCode);
                if (res != null) return res;
            }
            return _mapper.Map<CustomerBasketDTO>(updatedBasket);
        }
        /// <inheritdoc/>
        public async Task<CustomerBasketDTO?> AddItemToBasketAsync(BasketAddRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.BasketItem.Id);
            if (product is null || product.QuantityAvailable < request.BasketItem.Qunatity || request.BasketItem.Qunatity <= 0)
                return null;

            var newItem = new BasketItem
            {
                Id = request.BasketItem.Id,
                Name = product.Name,
                Price = product.NewPrice,
                Description = product.Description,
                Qunatity = request.BasketItem.Qunatity,
                Category = request.BasketItem.Category,
                Image = request.BasketItem.Image,
            };

            var existingBasket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(request.BasketId);

            if (existingBasket != null)
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
                existingBasket.Total += (newItem.Price * newItem.Qunatity);

                var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(existingBasket);
                if (existingBasket.DiscountId.HasValue && !string.IsNullOrEmpty(existingBasket.DiscountCode))
                {
                    var res = await ApplyDiscountAsync(existingBasket.Id, existingBasket.DiscountCode);
                    if (res != null) return res;
                }
                return _mapper.Map<CustomerBasketDTO>(updatedBasket);
            }
            else
            {
                var newBasket = new CustomerBasket(request.BasketId)
                {
                    BasketItems = new List<BasketItem> { newItem },
                    Total = (newItem.Price * newItem.Qunatity)
                };

                var basketResponse = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(newBasket);
                return _mapper.Map<CustomerBasketDTO>(basketResponse);
            }
        }

        public async Task<CustomerBasketDTO?> ApplyDiscountAsync(string basketId, string discountCode)
        {
            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            var discount = await _unitOfWork.DiscountRepository.GetByCodeAsync(discountCode);
            if (discount == null || discount.Status != DiscountStatus.Active) return null;

            if (discount.CurrentUsageCount >= discount.MaxUsageCount) return null;

            decimal eligibleTotal = 0m;
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                if (product == null) continue;
                bool applies = discount.DiscountType switch
                {
                    DiscountType.Product => discount.ProductId == product.Id,
                    DiscountType.Category => discount.CategoryId == product.CategoryId,
                    DiscountType.Brand => discount.BrandId == product.BrandId,
                    DiscountType.Global => true,
                    _ => false
                };

                if (applies)
                {
                    eligibleTotal += item.Price * item.Qunatity;
                }
            }

            if (eligibleTotal == 0) return null;

            var discountAmount = Math.Round(eligibleTotal * (discount.Percentage / 100m), 2, MidpointRounding.AwayFromZero);

            basket.DiscountCode = discount.Code;
            basket.DiscountId = discount.Id;
            basket.DiscountValue = discountAmount;
            basket.Percentage = discount.Percentage;

            var updatedBasket = await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
            return _mapper.Map<CustomerBasketDTO>(updatedBasket);
        }

    }
}
