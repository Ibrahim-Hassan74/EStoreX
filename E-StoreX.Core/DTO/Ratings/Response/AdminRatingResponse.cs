namespace EStoreX.Core.DTO.Ratings.Response
{
    public class AdminRatingResponse
    {
        public Guid RatingId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string CommentContent { get; set; } = string.Empty;
        public int Score { get; set; }
    }
}
