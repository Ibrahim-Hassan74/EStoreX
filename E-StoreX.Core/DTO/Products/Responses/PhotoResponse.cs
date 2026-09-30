namespace EStoreX.Core.DTO.Products.Responses
{
    public class PhotoResponse
    {
        public string ImageName { get; set; }
        public override string ToString()
            => ImageName;
    }

    public class PhotoResponseWithDetails
    {
        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public override string ToString()
            => ImageName;
    }

}
