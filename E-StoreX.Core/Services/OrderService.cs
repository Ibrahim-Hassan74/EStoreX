using AutoMapper;
using EStoreX.Core.DTO;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Core.ServiceContracts;

namespace EStoreX.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = _unitOfWork.OrderRepository;
        }

        public Task<OrderResponse> CreateOrdersAsync(OrderAddRequest order, string BuyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(string BuyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse> GetOrderByIdAsync(Guid Id, string BuyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
