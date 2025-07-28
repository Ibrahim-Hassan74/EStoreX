using System.ComponentModel.DataAnnotations;
namespace EStoreX.Core.DTO
{
    public class OrderAddRequest
    {
        [Required(ErrorMessage = "Delivery method ID is required.")]
        public Guid DeliveryMethodId { get; set; }

        [Required(ErrorMessage = "Basket ID is required.")]
        [StringLength(100, ErrorMessage = "Basket ID cannot exceed 100 characters.")]
        public string BasketId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping address is required.")]
        public ShippingAddressDTO ShippingAddress { get; set; }
    }
}