namespace EStoreX.Core.DTO.Products.Responses
{
    public class PhotoResponse
    {
        public string ImageName { get; set; }
        public override string ToString()
            => ImageName;
    }

}
