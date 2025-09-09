using EStoreX.Core.Enums;

namespace EStoreX.Core.DTO.Discounts.Responses
{
    public class DiscountResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal Percentage { get; set; }
        public DiscountType DiscountType { get; set; }
        public DiscountStatus Status { get; set; }

        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }

        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public Guid? BrandId { get; set; }
        public string? BrandName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
