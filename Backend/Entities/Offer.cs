using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGH.Domain.Entities;

public class Offer
{
#pragma warning disable CS8632
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    public string? Location { get; set; }

    [Required]
    public byte[] ImageData { get; set; }
    public string ImageMimeType { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Contact { get; set; }
    public string? Accomodation { get; set; }
    public string? accomodationsuitable { get; set; }

    [Required]
    public string skills { get; set; }
    public string? country { get; set; }
    public string? state { get; set; }
    public string? city { get; set; }
    public Guid HostId { get; set; }

    [ForeignKey("HostId")]
    public User User { get; set; }
    public ICollection<OfferApplication> OfferApplications { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public double AverageRating
    {
        get
        {
            if (Reviews == null || !Reviews.Where(r => r.ReviewedId == HostId).Any())
            {
                return 0.0;
            }

            return Math.Round(Reviews.Where(r => r.ReviewedId == HostId).Average(r => r.RatingValue), 1);
        }
    }

    public enum Fb_Status
    {
        pending,
        posted
    }
}
