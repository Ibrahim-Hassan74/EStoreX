using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO
{
    public record ProductRequest
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "{0} can't be blank")]
        [StringLength(1000, ErrorMessage = "{0} must be between {1} and {2}", MinimumLength = 5)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        public string CategoryName { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "At least one photo is required.")]
        public List<PhotoRequest> Photos { get; set; } = new();
    }

}
