using Domain.Entities.Baskets;
using EStoreX.Core.RepositoryContracts.Basket;
using StackExchange.Redis;
using System.Text.Json;

namespace EStoreX.Infrastructure.Repository.Basket
{
    public class CustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly IDatabase _db;
        public CustomerBasketRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        /// <inheritdoc/>
        public Task<bool> DeleteBasketAsync(string Id)
        {
            return _db.KeyDeleteAsync(Id);
        }

        /// <inheritdoc/>
        public async Task<CustomerBasket?> GetBasketAsync(string Id)
        {
            var result = await _db.StringGetAsync(Id);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerBasket>(result);
            }
            return null;
        }

        /// <inheritdoc/>
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var saved = await _db.StringSetAsync(
                basket.Id,
                JsonSerializer.Serialize(basket),
                TimeSpan.FromDays(3)
            );

            return saved ? await GetBasketAsync(basket.Id) : null;
        }

    }
}
