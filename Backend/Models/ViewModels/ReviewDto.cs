using UGH.Domain.Entities;
using UGHApi.ViewModels.UserComponent;
namespace UGHApi.ViewModels;

public class OfferDto
{
#pragma warning disable CS8632
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly CreatedAt { get; set; }
    public DateOnly ModifiedAt { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public int GroupSize { get; set; }
    public string? GroupProperties { get; set; }
    public string Requirements { get; set; }
    public MobilityEnum Mobility { get; set; }
    public OfferType OfferType { get; set; }
    public string Skills { get; set; }
    public OfferStatus Status { get; set; }
    public UserC User { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string? DeletedOfferTitle { get; set; }
}

public class ReviewDto
{
#pragma warning disable CS8632
    public int Id { get; set; }
    public int? OfferId { get; set; }  // Optional: Referenz auf das Angebot
    public int RatingValue { get; set; }
    public string? ReviewComment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserC Reviewer { get; set; }
    public UserC Reviewed { get; set; }
    public OfferDto? Offer { get; set; }  // Optional: Kann null sein
}
