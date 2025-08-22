using Domain.Entities.Common;
using EStoreX.Core.Domain.Entities.Rating;
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
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative.")]
        public int QuantityAvailable { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        public Guid BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public virtual Brand Brand { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    }
}