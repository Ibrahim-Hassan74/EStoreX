using System.ComponentModel.DataAnnotations;

namespace SufraX.Core.Domain.Entities
{
    public class Ingredient
    {
        [Key]
        public Guid IngredientId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(800)]
        public string? Description { get; set; }
        public ICollection<ProductIngredient>? ProductIngredients { get; set; }
        public Ingredient()
        {
            ProductIngredients = new HashSet<ProductIngredient>();
        }
    }
}