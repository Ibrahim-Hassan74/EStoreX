using SufraX.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace SufraX.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Status { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
