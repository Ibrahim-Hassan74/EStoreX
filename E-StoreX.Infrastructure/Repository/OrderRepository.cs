using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.Data;

namespace EStoreX.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <inheritDoc />
        public Task<Order> CreateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        /// <inheritDoc />
        public Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritDoc />
        public Task<DeliveryMethod?> GetDeliveryMethodByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <inheritDoc />
        public Task<Order?> GetOrderByIdAsync(Guid orderId, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        /// <inheritDoc />
        public Task<IEnumerable<Order>> GetOrdersByBuyerEmailAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
