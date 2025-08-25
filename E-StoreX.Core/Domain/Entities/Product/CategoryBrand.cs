namespace Domain.Entities.Product
{
    public class CategoryBrand
    {
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
