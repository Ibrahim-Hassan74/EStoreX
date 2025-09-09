using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Discount.Request;

namespace EStoreX.Core.ServiceContracts.Discount
{
    public interface IDiscountService
    {
        Task<ApiResponse> CreateDiscountAsync(DiscountRequest request);
        Task<ApiResponse> UpdateDiscountAsync(Guid id, DiscountRequest request);
        Task<ApiResponse> DeleteDiscountAsync(Guid id);
        Task<ApiResponse> GetDiscountByIdAsync(Guid id);
        Task<ApiResponse> GetDiscountByCodeAsync(string code);
        Task<ApiResponse> GetAllDiscountsAsync();
        Task<ApiResponse> GetActiveDiscountsAsync();
        Task<ApiResponse> GetExpiredDiscountsAsync();
        Task<ApiResponse> GetNotStartedDiscountsAsync();
        Task<ApiResponse> ActivateDiscountAsync(Guid id);
        Task<ApiResponse> ExpireDiscountAsync(Guid id);
        Task<ApiResponse> UpdateDiscountDatesAsync(Guid id, DateTime startDate, DateTime? endDate);
        Task<ApiResponse> ApplyDiscountToProductAsync(Guid productId, string code);
        Task<ApiResponse> ValidateDiscountCodeAsync(string code);
    }

}
