using EStoreX.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class DiscountRequest : IValidatableObject
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
    [Range(1, int.MaxValue, ErrorMessage = "MaxUsageCount must be at least 1.")]
    public int MaxUsageCount { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        switch (DiscountType)
        {
            case DiscountType.Product:
                if (!ProductId.HasValue)
                    yield return new ValidationResult("ProductId is required when DiscountType is Product.", new[] { nameof(ProductId) });
                break;
            case DiscountType.Category:
                if (!CategoryId.HasValue)
                    yield return new ValidationResult("CategoryId is required when DiscountType is Category.", new[] { nameof(CategoryId) });
                break;
            case DiscountType.Brand:
                if (!BrandId.HasValue)
                    yield return new ValidationResult("BrandId is required when DiscountType is Brand.", new[] { nameof(BrandId) });
                break;
        }
    }
}
