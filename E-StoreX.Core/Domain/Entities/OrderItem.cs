using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EStoreX.Core.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [NotMapped]
        public decimal TotalPrice => Quantity * Price;
    }
}
