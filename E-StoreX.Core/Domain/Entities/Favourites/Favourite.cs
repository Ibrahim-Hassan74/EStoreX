using System.ComponentModel.DataAnnotations;
using EStoreX.Core.Domain.IdentityEntities;
using Domain.Entities.Product;

namespace EStoreX.Core.Domain.Entities.Favourites
{
    public class Favourite
    {
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}
