using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Product;

namespace EStoreX.Core.Domain.Entities
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }

        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [NotMapped]
        public decimal TotalPrice => Product.Price.GetValueOrDefault() * Quantity;
    }

}