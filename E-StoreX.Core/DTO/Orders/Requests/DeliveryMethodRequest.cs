using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Orders.Requests
{
    public class DeliveryMethodRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "DeliveryTime is required.")]
        [StringLength(50, ErrorMessage = "DeliveryTime can't be longer than 50 characters.")]
        public string DeliveryTime { get; set; } = string.Empty;
    }
}
