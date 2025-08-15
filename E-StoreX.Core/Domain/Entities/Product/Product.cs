using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class Product : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal NewPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal OldPrice { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public List<Photo> Photos { get; set; } = new List<Photo>();

    }
}