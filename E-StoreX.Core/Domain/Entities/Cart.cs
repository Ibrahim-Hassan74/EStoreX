using EStoreX.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.Domain.Entities
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
