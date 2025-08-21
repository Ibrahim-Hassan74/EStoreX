namespace EStoreX.Core.DTO.Ratings.Response
{
    public class RatingResponse
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        public Guid ProductId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
