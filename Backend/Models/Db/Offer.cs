using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGH.Domain.Entities;

public class Offer {
#pragma warning disable CS8632
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    // pictures can be used by multiple offers. If an offer is deleted the linked picture might need to persist.
    public int? PictureId { get; set; }
    [ForeignKey("PictureId")]
    public virtual Picture Picture { get; set; }

    public DateOnly CreatedAt { get; set; }
    // add modification date (before migration)
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }

    // number of adults. Usually one or 2. (for non-requests: maximum, for requests: intended group size)
    public int GroupSize { get; set; }
    // pets, children, etc
    public string? GroupProperties { get; set;}

    // in Case of Lodging for HouseRules otherwise for preferences
    public String Requirements { get; set; }
    
    public MobilityEnum Mobility { get; set;}
    public OfferType OfferType { get; set; }
    
    [Required]
    public string Skills { get; set; }    

    [Required]
    public OfferStatus Status { get; set; }

    [Required]
    public Guid UserId { get; init; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public ICollection<OfferApplication> OfferApplications { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

public class OfferTypeLodging : Offer {    
    public string? LodgingType { get; set; }
    public string? AdditionalLodgingProperties { get; set; }
    public string? Location { get; set; }
}
public class OfferTypeRequest : Offer {
    // differs from Location, has to be treated as a list. Multiple Locations are realistic.
    public string? PossibleLocations { get; set; }
    // health or other important individual factors. Not searchable.
    public string? SpecialConditions { get; set; }
}

public enum MobilityEnum {
    IndividualJourney,
    GuestPickedUp
}
public enum OfferType {
    Lodging,
    Request
}
public enum OfferStatus {
    Active,
    Expired,
    Withdrawn,
    Updated
}
