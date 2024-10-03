using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGH.Domain.Entities;

public class OfferApplication
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OfferId { get; set; }

    [ForeignKey("OfferId")]
    public Offer Offer { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [Required]
    public Guid HostId { get; set; }

    [ForeignKey("HostId")]
    public User Host { get; set; }

    [Required]
    public OfferApplicationStatus Status { get; set; } = OfferApplicationStatus.Pending;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum OfferApplicationStatus
{
    Pending,
    Approved,
    Rejected
}
