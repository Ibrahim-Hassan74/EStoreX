using EStoreX.Core.RepositoryContracts.Common;
using Domain.Entities.Product;

namespace EStoreX.Core.RepositoryContracts.Discounts
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<Discount?> GetByCodeAsync(string code);
        Task<IEnumerable<Discount>> GetActiveDiscountsAsync();
        Task<IEnumerable<Discount>> GetExpiredDiscountsAsync();
        Task<IEnumerable<Discount>> GetNotStartedDiscountsAsync();
        Task<Discount?> GetActiveDiscountByCodeAsync(string code);
    }

}
