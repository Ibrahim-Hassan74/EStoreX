namespace EStoreX.Core.DTO.Ratings.Response
{
    public class ProductRatingResponse
    {
        public Guid ProductId { get; set; }
        public double AverageScore { get; set; }
        public int TotalRatings { get; set; }
        public Dictionary<int, int>? ScoreDistribution { get; set; }
    }
}
