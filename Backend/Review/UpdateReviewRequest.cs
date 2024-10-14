using System.ComponentModel.DataAnnotations;

namespace UGH.Contracts.Review;

public class UpdateReviewRequest
{
#pragma warning disable CS8632
    [Required]
    public int ReviewId { get; set; }

    [Range(1, 5, ErrorMessage = "RatingValue must be between 1 and 5.")]
    public int RatingValue { get; set; }

    public string? ReviewComment { get; set; }
}
