using EStoreX.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class Product : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        public List<Photo> Photos { get; set; }

    }
}