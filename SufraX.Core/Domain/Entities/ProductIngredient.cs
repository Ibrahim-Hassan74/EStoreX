namespace SufraX.Core.Domain.Entities
{
    public class ProductIngredient
    {
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}