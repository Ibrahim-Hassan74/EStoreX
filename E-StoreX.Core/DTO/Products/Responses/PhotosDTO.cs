using Domain.Entities.Product;
using Microsoft.AspNetCore.Http;


namespace EStoreX.Core.DTO.Products.Responses
{
    public class PhotosDTO
    {
        public IFormFileCollection FormFiles { get; set; }
        public string Src { get; set; }
        public Guid ProductId { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
