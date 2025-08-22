using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities.Product
{
    public class Brand : BaseEntity<Guid>
    {
        [Required(ErrorMessage = "Brand Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
