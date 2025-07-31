using UGH.Domain.Entities;
using UGHApi.ViewModels.UserComponent;
namespace UGHApi.ViewModels;

public class ReviewDto
{
#pragma warning disable CS8632
    public int Id { get; set; }
    public int? OfferId { get; set; }  // Optional: Kann null sein wenn Offer gelöscht wurde
    public int RatingValue { get; set; }
    public string? ReviewComment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ReviewerId { get; set; }
    public Guid ReviewedId { get; set; }
    public UserC Reviewer { get; set; }
    public UserC Reviewed { get; set; }
    public Offer? Offer { get; set; } // Optional: Kann null sein wenn Offer gelöscht wurde
    
    // Reviewer-Informationen (gespeichert für den Fall, dass der User sich löscht)
    public string? ReviewerFirstName { get; set; }
    public string? ReviewerLastName { get; set; }
    public string? ReviewerEmail { get; set; }
    
    // Sichtbarkeitslogik
    public bool IsVisible { get; set; }
    public DateTime? VisibilityDate { get; set; }
}
