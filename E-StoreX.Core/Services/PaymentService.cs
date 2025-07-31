using EStoreX.Core.Domain.Entities;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace EStoreX.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId, Guid? deliveryMethodId)
        {
            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basketId);
            if (basket is null)
                throw new Exception($"Basket with ID {basketId} not found.");

            StripeConfiguration.ApiKey = _configuration["StripeSetting:SecretKey"];
            decimal shippingPrice = 0m;
            if (deliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.OrderRepository.GetDeliveryMethodByIdAsync(deliveryMethodId.Value);
                if (deliveryMethod is not null)
                    shippingPrice = deliveryMethod.Price;
            }
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                if(product is null)
                    throw new Exception($"Product with ID {item.Id} not found.");
                item.Price = product.NewPrice;
            }

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent _intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)((basket.BasketItems.Sum(x => x.Qunatity * (x.Price * 100)) + (shippingPrice * 100))),
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" },
                };
                _intent = await service.CreateAsync(options);
                basket.PaymentIntentId = _intent.Id;
                basket.ClientSecret = _intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)((basket.BasketItems.Sum(x => x.Qunatity * (x.Price * 100)) + (shippingPrice * 100))),
                };
                _intent = await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public Task<CustomerBasket> GetPaymentIntentAsync(string basketId)
        {
            throw new NotImplementedException();
        }
    }
}
