using AutoMapper;
using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;

namespace EStoreX.Core.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentService _paymentService;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService) : base(unitOfWork, mapper)
        {
            _orderRepository = _unitOfWork.OrderRepository;
            _paymentService = paymentService;
        }
        /// <inheritdoc/>
        public async Task<OrderResponse> CreateOrdersAsync(OrderAddRequest order, string buyerEmail)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            }
            if (string.IsNullOrEmpty(buyerEmail))
            {
                throw new ArgumentException("Buyer email cannot be null or empty", nameof(buyerEmail));
            }

            var basket = await _unitOfWork.CustomerBasketRepository.GetBasketAsync(order.BasketId);
            if (basket == null)
            {
                throw new InvalidOperationException("Basket not found");
            }
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem
                (
                    item.Price,
                    item.Qunatity,
                    item.Id,
                    item.Image,
                    product?.Name ?? "Unknown Product"
                );
                orderItems.Add(orderItem);
            }

            var deliveryMethod = await _orderRepository.GetDeliveryMethodByIdAsync(order.DeliveryMethodId);
            if (deliveryMethod == null)
                throw new InvalidOperationException("Delivery method not found");

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            var shippingAddress = _mapper.Map<ShippingAddress>(order.ShippingAddress);

            var existingOrder = await _orderRepository.GetOrderByPaymentIntentIdAsync(basket.PaymentIntentId);

            if (existingOrder is not null)
            {
                await _orderRepository.DeleteOrderAsync(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntentAsync(basket.Id, deliveryMethod.Id);
            }

            var orderEntity = new Order(buyerEmail, subTotal, shippingAddress, deliveryMethod, orderItems, basket.PaymentIntentId);
            var createdOrder = await _orderRepository.CreateOrderAsync(orderEntity);

            await _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(order.BasketId);

            return _mapper.Map<OrderResponse>(createdOrder);
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(string buyerEmail)
        {
            if (string.IsNullOrWhiteSpace(buyerEmail))
            {
                throw new ArgumentException("Buyer email cannot be null, empty, or whitespace", nameof(buyerEmail));
            }

            var orders = await _orderRepository.GetOrdersByBuyerEmailAsync(buyerEmail);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodAsync()
        {
            var deliveryMethods = await _orderRepository.GetAllDeliveryMethodsAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }

        /// <inheritdoc/>
        public async Task<OrderResponse> GetOrderByIdAsync(Guid Id, string buyerEmail)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(Id));
            }
            if (string.IsNullOrWhiteSpace(buyerEmail))
            {
                throw new ArgumentException("Buyer email cannot be null, empty, or whitespace", nameof(buyerEmail));
            }
            var order = await _orderRepository.GetOrderByIdAsync(Id, buyerEmail);
            return _mapper.Map<OrderResponse>(order);
        }
    }
}
