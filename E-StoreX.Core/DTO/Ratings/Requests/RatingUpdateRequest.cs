using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Ratings.Requests
{
    public class RatingUpdateRequest
    {
        [Range(1, 5, ErrorMessage = "Score must be between 1 and 5.")]
        public int Score { get; set; }

        [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string? Comment { get; set; }
    }

}
