using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public Category? Category { get; set; }
        public Guid CategoryId { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

    }
}