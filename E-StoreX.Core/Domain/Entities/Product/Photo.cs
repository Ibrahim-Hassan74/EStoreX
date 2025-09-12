using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class Photo : BaseEntity<Guid>
    {
        public string ImageName { get; set; }
        public Guid? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
        public Guid? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
        public Guid? BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public virtual Brand? Brand { get; set; }
    }

}
