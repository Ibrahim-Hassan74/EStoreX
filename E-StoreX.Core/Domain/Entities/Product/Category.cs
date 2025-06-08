using EStoreX.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Product
{
    public class Category : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();    
    }
}