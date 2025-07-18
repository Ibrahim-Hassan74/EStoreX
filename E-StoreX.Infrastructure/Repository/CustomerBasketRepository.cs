using EStoreX.Core.Domain.Entities;
using EStoreX.Core.RepositoryContracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EStoreX.Infrastructure.Repository
{
    public class CustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly IDatabase _db;
        public CustomerBasketRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public Task<bool> DeleteBasketAsync(string Id)
        {
            return _db.KeyDeleteAsync(Id);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string Id)
        {
            var result = await _db.StringGetAsync(Id);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerBasket>(result);
            }
            return null;
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var _basket = await _db.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));
            if (_basket)
            {
                return await GetBasketAsync(basket.Id);
            }
            return null;
        }
    }
}
