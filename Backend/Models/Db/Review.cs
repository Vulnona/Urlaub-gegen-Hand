using System.ComponentModel.DataAnnotations.Schema;

namespace UGH.Domain.Entities;

public class Review
{
#pragma warning disable CS8632
    public int Id { get; set; }
    public int? OfferId { get; set; }  // Optional: Kann null sein wenn Offer gelöscht wird
    public int RatingValue { get; set; }
    public string? ReviewComment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ReviewerId { get; set; }
    public Guid ReviewedId { get; set; }

    [ForeignKey("OfferId")]
    public Offer? Offer { get; set; } // Optional: Kann null sein

    [ForeignKey("ReviewerId")]
    public User Reviewer { get; set; }

    [ForeignKey("ReviewedId")]
    public User Reviewed { get; set; }
}

public enum reviewStatus
{
    Pending,
    Approved,
    Rejected
}
