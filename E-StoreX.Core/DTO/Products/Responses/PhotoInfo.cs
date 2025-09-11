using Domain.Entities.Common;

namespace EStoreX.Core.DTO.Products.Responses
{
    public class PhotoInfo : BaseEntity<Guid>
    {
        public string ImageName { get; set; }
    }
}
