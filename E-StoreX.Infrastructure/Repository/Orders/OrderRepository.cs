using EStoreX.Core.Domain.Entities.Orders;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EStoreX.Core.RepositoryContracts.Orders;
using EStoreX.Infrastructure.Repository.Common;

namespace EStoreX.Infrastructure.Repository.Orders
{
    /// <summary>
    /// Provides implementation for order data access using Entity Framework Core.
    /// </summary>
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <inheritdoc />
        public async Task<IEnumerable<Order>> GetOrdersByBuyerEmailAsync(string buyerEmail)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.ShippingAddress)
                .Include(o => o.DeliveryMethod)
                .Where(o => o.BuyerEmail == buyerEmail)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Order?> GetOrderByIdAsync(Guid orderId, string buyerEmail)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.ShippingAddress)
                .Include(o => o.DeliveryMethod)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.BuyerEmail == buyerEmail);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        {
            return await _context.DeliveryMethods.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<DeliveryMethod?> GetDeliveryMethodByIdAsync(Guid id)
        {
            return await _context.DeliveryMethods.FirstOrDefaultAsync(dm => dm.Id == id);
        }
        /// <inheritdoc />
        public async Task<Order?> GetOrderByPaymentIntentIdAsync(string? paymentIntentId)
        {
            if (string.IsNullOrEmpty(paymentIntentId))
            {
                return null;
            }
            return await _context.Orders.Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);
        }
        /// <inheritdoc />
        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) 
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();
        }
    }
}
