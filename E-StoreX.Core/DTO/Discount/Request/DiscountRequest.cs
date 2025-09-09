using EStoreX.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Discount.Request
{
    public class DiscountRequest
    {
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
        public decimal Percentage { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }
    }

}
