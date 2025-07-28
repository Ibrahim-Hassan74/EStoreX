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
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _orderRepository = _unitOfWork.OrderRepository;
        }

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

            var orderEntity = new Order(buyerEmail, subTotal, shippingAddress, deliveryMethod, orderItems);
            var createdOrder = await _orderRepository.CreateOrderAsync(orderEntity);

            await _unitOfWork.CustomerBasketRepository.DeleteBasketAsync(order.BasketId);

            return _mapper.Map<OrderResponse>(createdOrder);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(string buyerEmail)
        {
            if (string.IsNullOrWhiteSpace(buyerEmail))
            {
                throw new ArgumentException("Buyer email cannot be null, empty, or whitespace", nameof(buyerEmail));
            }

            var orders = await _orderRepository.GetOrdersByBuyerEmailAsync(buyerEmail);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse> GetOrderByIdAsync(Guid Id, string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
