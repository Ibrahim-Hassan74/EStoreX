using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Products.Requests
{
    public class ProductAddRequest
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "{0} can't be blank")]
        [StringLength(1000, ErrorMessage = "{0} must be between {1} and {2}", MinimumLength = 5)]
        public string Description { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal NewPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal OldPrice { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative.")]
        public int QuantityAvailable { get; set; }
        public Guid CategoryId { get; set; }
        [MinLength(1, ErrorMessage = "At least one photo is required.")]
        public IFormFileCollection Photos { get; set; }
    }

}
