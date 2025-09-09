using Domain.Entities.Common;
using EStoreX.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class Discount : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public decimal Percentage { get; set; }

        public DiscountType DiscountType { get; set; }

        public Guid? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        public Guid? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        public Guid? BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public virtual Brand? Brand { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }
        public int MaxUsageCount { get; set; } = 100;

        public int CurrentUsageCount { get; set; } = 0;

        [NotMapped]
        public DiscountStatus Status
        {
            get
            {
                if (StartDate > DateTime.UtcNow)
                    return DiscountStatus.NotStarted;

                if (EndDate.HasValue && EndDate.Value < DateTime.UtcNow)
                    return DiscountStatus.Expired;

                if (CurrentUsageCount >= MaxUsageCount)
                    return DiscountStatus.Expired;

                return DiscountStatus.Active;
            }
        }
    }
}
