using Domain.Entities.Baskets;
using EStoreX.Core.Domain.Options;
using EStoreX.Core.Enums;
using Microsoft.Extensions.Options;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Common;
using Stripe;
using MyProduct = Domain.Entities.Product.Product;

namespace EStoreX.Core.Services.Common
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IConfiguration _configuration;
        private readonly StripeSettings _stripeSettings;
        private readonly PaymentIntentService _paymentIntentService;
        public PaymentService(IUnitOfWork unitOfWork, IOptions<StripeSettings> options, PaymentIntentService paymentIntentService)
        {
            _unitOfWork = unitOfWork;
            //_configuration = configuration;
            _stripeSettings = options.Value;
            _paymentIntentService = paymentIntentService;
        }
        /// <inheritdoc/>
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId, Guid? deliveryMethodId)
        {
            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(basketId);
            if (basket is null)
                throw new Exception($"Basket with ID {basketId} not found.");

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey; // _configuration["StripeSetting:SecretKey"];
            decimal shippingPrice = 0m;
            if (deliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.OrderRepository.GetDeliveryMethodByIdAsync(deliveryMethodId.Value);
                if (deliveryMethod is not null)
                    shippingPrice = deliveryMethod.Price;
            }
            decimal total = 0m;
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                if (product is null)
                    throw new Exception($"Product with ID {item.Id} not found.");
                if(product.QuantityAvailable < item.Qunatity)
                    throw new Exception($"Not enough stock for product {product.Name}");

                item.Price = product.NewPrice;
                var (unitPrice, discountAmount) = await GetDiscountedPriceAsync(product, item.Qunatity, basket);
                total += unitPrice * item.Qunatity;
            }

            //PaymentIntentService service = new PaymentIntentService();
            PaymentIntent _intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)((total + shippingPrice) * 100),
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" },
                };
                _intent = await _paymentIntentService.CreateAsync(options);
                basket.PaymentIntentId = _intent.Id;
                basket.ClientSecret = _intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)((total + shippingPrice) * 100),
                };
                _intent = await _paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _unitOfWork.CustomerBasketRepository.UpdateBasketAsync(basket);
            return basket;
        }
        /// <inheritdoc/>
        public async Task<bool> UpdateOrderFailedAsync(string? paymentIntentId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByPaymentIntentIdAsync(paymentIntentId);
            if (order is null) return false;

            order.Status = Status.PaymentFailed;
            await _unitOfWork.CompleteAsync();
            return true;
        }
        /// <inheritdoc/>
        public async Task<bool> UpdateOrderSuccessAsync(string? paymentIntentId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByPaymentIntentIdAsync(paymentIntentId);
            if (order is null) return false;

            if (order.Status != Status.Pending)
                throw new InvalidOperationException("Order is not in a pending state.");

            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductItemId);
                if (product == null) continue;

                if (product.QuantityAvailable < item.Quantity)
                    throw new InvalidOperationException($"Not enough stock for product {product.Name}");

                product.QuantityAvailable -= item.Quantity;
                await _unitOfWork.ProductRepository.UpdateAsync(product);
            }

            if (order.DiscountId.HasValue)
            {
                var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(order.DiscountId.Value);
                if (discount is not null)
                {
                    discount.CurrentUsageCount++;
                    await _unitOfWork.DiscountRepository.UpdateAsync(discount);
                }
            }

            order.Status = Status.PaymentReceived;
            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task<(decimal unitPrice, decimal discountAmount)> GetDiscountedPriceAsync(MyProduct product, int quantity, CustomerBasket basket)
        {
            if (basket.DiscountId == null)
                return (product.NewPrice, 0m);

            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(basket.DiscountId.Value);
            if (discount == null || discount.Status != DiscountStatus.Active)
                return (product.NewPrice, 0m);

            var now = DateTime.UtcNow;
            if (discount.StartDate > now || (discount.EndDate.HasValue && discount.EndDate.Value < now))
                return (product.NewPrice, 0m);

            bool applies = discount.DiscountType switch
            {
                DiscountType.Product => discount.ProductId == product.Id,
                DiscountType.Category => discount.CategoryId == product.CategoryId,
                DiscountType.Brand => discount.BrandId == product.BrandId,
                DiscountType.Global => true,
                _ => false
            };

            if (!applies)
                return (product.NewPrice, 0m);

            var discountedUnitPrice = product.NewPrice - (product.NewPrice * (discount.Percentage / 100m));
            var discountAmount = (product.NewPrice - discountedUnitPrice) * quantity;

            return (
                Math.Round(discountedUnitPrice, 2, MidpointRounding.AwayFromZero),
                Math.Round(discountAmount, 2, MidpointRounding.AwayFromZero)
            );
        }



    }
}
