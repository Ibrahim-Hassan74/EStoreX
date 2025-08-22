using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities.Product
{
    public class Category : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}