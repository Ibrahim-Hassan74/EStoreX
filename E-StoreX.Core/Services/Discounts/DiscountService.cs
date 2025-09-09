using AutoMapper;
using Domain.Entities.Product;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.DTO.Discount.Request;
using EStoreX.Core.DTO.Discount.Response;
using EStoreX.Core.DTO.Discounts.Responses;
using EStoreX.Core.Enums;
using EStoreX.Core.Helper;
using EStoreX.Core.RepositoryContracts.Common;
using EStoreX.Core.ServiceContracts.Discount;
using EStoreX.Core.Services.Common;

namespace EStoreX.Core.Services.Discounts
{
    public class DiscountService : BaseService, IDiscountService
    {
        public DiscountService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
        public async Task<ApiResponse> CreateDiscountAsync(DiscountRequest request)
        {
            if (request.EndDate.HasValue && request.EndDate <= request.StartDate)
                return ApiResponseFactory.BadRequest("End date must be greater than start date");

            var discount = _mapper.Map<Discount>(request);
            discount.Code = Guid.NewGuid().ToString("N")[..8].ToUpper();

            await _unitOfWork.DiscountRepository.AddAsync(discount);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<DiscountResponse>(discount);
            return ApiResponseFactory.Success("Discount created successfully", response);
        }

        public async Task<ApiResponse> UpdateDiscountAsync(Guid id, DiscountRequest request)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
            if (discount == null)
                return ApiResponseFactory.NotFound("Discount not found");

            if (request.EndDate.HasValue && request.EndDate <= request.StartDate)
                return ApiResponseFactory.BadRequest("End date must be greater than start date");

            _mapper.Map(request, discount);

            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<DiscountResponse>(discount);
            return ApiResponseFactory.Success("Discount updated successfully", response);
        }

        public async Task<ApiResponse> DeleteDiscountAsync(Guid id)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
            if (discount == null)
                return ApiResponseFactory.NotFound("Discount not found");

            await _unitOfWork.DiscountRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return ApiResponseFactory.Success("Discount deleted successfully");
        }

        public async Task<ApiResponse> GetDiscountByIdAsync(Guid id)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id, x => x.Product, c => c.Category, b => b.Brand);
            if (discount == null)
                return ApiResponseFactory.NotFound("Discount not found");

            if (discount.Status != DiscountStatus.Active)
                return ApiResponseFactory.BadRequest("Discount code is not active");

            var response = _mapper.Map<DiscountResponse>(discount);
            return ApiResponseFactory.Success("Discount retrieved successfully", response);
        }

        public async Task<ApiResponse> GetDiscountByCodeAsync(string code)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByCodeAsync(code);
            if (discount == null)
                return ApiResponseFactory.NotFound("Discount not found");

            if (discount.Status != DiscountStatus.Active)
                return ApiResponseFactory.BadRequest("Discount code is not active");

            var response = _mapper.Map<DiscountResponse>(discount);
            return ApiResponseFactory.Success("Discount retrieved successfully", response);
        }

        public async Task<ApiResponse> GetAllDiscountsAsync()
        {
            var discounts = await _unitOfWork.DiscountRepository.GetAllAsync(x => x.Product, c => c.Category, b => b.Brand);
            var mapped = _mapper.Map<List<DiscountResponse>>(discounts);
            return ApiResponseFactory.Success("Discounts retrieved successfully", mapped);
        }

        public async Task<ApiResponse> GetActiveDiscountsAsync()
        {
            var discounts = await _unitOfWork.DiscountRepository.GetActiveDiscountsAsync();
             
            var mapped = _mapper.Map<List<DiscountResponse>>(discounts);
            return ApiResponseFactory.Success("Active discounts retrieved successfully", mapped);
        }

        public async Task<ApiResponse> GetExpiredDiscountsAsync()
        {
            var discounts = await _unitOfWork.DiscountRepository.GetExpiredDiscountsAsync();
            var mapped = _mapper.Map<List<DiscountResponse>>(discounts);
            return ApiResponseFactory.Success("Expired discounts retrieved successfully", mapped);
        }


        public async Task<ApiResponse> GetNotStartedDiscountsAsync()
        {
            var discounts = await _unitOfWork.DiscountRepository.GetNotStartedDiscountsAsync();
            var mapped = _mapper.Map<List<DiscountResponse>>(discounts);
            return ApiResponseFactory.Success("Upcoming discounts retrieved successfully", mapped);
        }

        public async Task<ApiResponse> ActivateDiscountAsync(Guid id)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
            if (discount == null)
                return ApiResponseFactory.NotFound("Discount not found");

            discount.StartDate = DateTime.UtcNow;
            discount.EndDate = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.DiscountRepository.UpdateAsync(discount);
            await _unitOfWork.CompleteAsync();

            return ApiResponseFactory.Success("Discount activated successfully");
        }

        public async Task<ApiResponse> ExpireDiscountAsync(Guid id)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
            if (discount == null)
                return ApiResponseFactory.NotFound("Discount not found");

            discount.EndDate = DateTime.UtcNow;

            await _unitOfWork.DiscountRepository.UpdateAsync(discount);
            await _unitOfWork.CompleteAsync();

            return ApiResponseFactory.Success("Discount expired successfully");
        }

        public async Task<ApiResponse> UpdateDiscountDatesAsync(Guid id, DateTime startDate, DateTime? endDate)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
            if (discount == null)
                return ApiResponseFactory.NotFound("Discount not found");

            discount.StartDate = startDate;
            discount.EndDate = endDate;

            await _unitOfWork.DiscountRepository.UpdateAsync(discount);
            await _unitOfWork.CompleteAsync();

            return ApiResponseFactory.Success("Discount dates updated successfully");
        }

        public async Task<ApiResponse> ApplyDiscountToProductAsync(Guid productId, string code)
        {
            var discount = await _unitOfWork.DiscountRepository.GetActiveDiscountByCodeAsync(code);

            if (discount == null)
                return ApiResponseFactory.BadRequest("Invalid or inactive discount code");

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, x => x.Category, y => y.Brand);
            if (product == null)
                return ApiResponseFactory.NotFound("Product not found");

            if (discount.ProductId.HasValue && discount.ProductId != product.Id)
                return ApiResponseFactory.BadRequest("Discount not valid for this product");

            if (discount.CategoryId.HasValue && discount.CategoryId != product.CategoryId)
                return ApiResponseFactory.BadRequest("Discount not valid for this category");

            if (discount.BrandId.HasValue && discount.BrandId != product.BrandId)
                return ApiResponseFactory.BadRequest("Discount not valid for this brand");

            var discountedPrice = product.NewPrice - (product.NewPrice * (discount.Percentage / 100m));

            var response = new AppliedDiscountResponse
            {
                Product = product.Name,
                OriginalPrice = product.NewPrice,
                DiscountedPrice = discountedPrice,
                DiscountPercentage = discount.Percentage
            };

            return ApiResponseFactory.Success("Discount applied successfully", response);

        }


        public async Task<ApiResponse> ValidateDiscountCodeAsync(string code)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByCodeAsync(code);

            if (discount == null)
                return ApiResponseFactory.BadRequest("Invalid discount code");

            if (discount.Status != DiscountStatus.Active)
                return ApiResponseFactory.BadRequest("Discount code is not active");

            return ApiResponseFactory.Success("Discount code is valid");
        }

    }
}
