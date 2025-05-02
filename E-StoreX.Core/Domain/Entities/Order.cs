using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        [Required]
        [MaxLength(200)]
        public string ShippingAddress { get; set; } = null!;
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        [MaxLength(50)]
        public string? Country { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
