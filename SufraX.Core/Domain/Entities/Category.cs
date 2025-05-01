using System.ComponentModel.DataAnnotations;

namespace SufraX.Core.Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        public ICollection<Product>? Products { get; set; }
        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}