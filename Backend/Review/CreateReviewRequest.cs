using System.ComponentModel.DataAnnotations;

namespace UGH.Contracts.Review;

public class CreateReviewRequest
{
#pragma warning disable CS8632
    [Required(ErrorMessage = "OfferId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "OfferId cannot be zero or negative.")]
    public int OfferId { get; set; }

    [Required(ErrorMessage = "RatingValue is required.")]
    [Range(1, 5, ErrorMessage = "RatingValue must be between 1 and 5.")]
    public int RatingValue { get; set; }

    public string? ReviewComment { get; set; }
    public Guid? ReviewedUserId { get; set; } = null;

}
