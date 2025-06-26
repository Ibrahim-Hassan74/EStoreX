using Microsoft.AspNetCore.Http;


namespace EStoreX.Core.DTO
{
    public class PhotosDTO
    {
        public IFormFileCollection formFiles { get; set; }
        public string Src { get; set; }
        public Guid ProductId { get; set; }
    }
}
