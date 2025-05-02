using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(300)]
        public string? Description { get; set; }
        public ICollection<Product>? Products { get; set; }
        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}