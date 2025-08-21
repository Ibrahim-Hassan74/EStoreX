using Domain.Entities.Common;
using Domain.Entities.Product;
using EStoreX.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.Domain.Entities.Rating
{
    public class Rating : BaseEntity<Guid>
    {

        [Range(1, 5)]
        public int Score { get; set; } // 1 - 5

        [MaxLength(500)]
        public string? Comment { get; set; }

        // Relations
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

}
