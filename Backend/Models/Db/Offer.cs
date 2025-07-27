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
    // Multi-Bild-Unterstützung über Pictures Collection

    public DateOnly CreatedAt { get; init; }
    public DateOnly ModifiedAt { get; set; }
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

    // Geographic location (NEW) - replacing old Location fields
    public int? AddressId { get; set; }
    [ForeignKey("AddressId")]
    public Address? Address { get; set; }

    [Required]
    public Guid UserId { get; init; }
    [ForeignKey("UserId")]
    public User User { get; set; }

    public ICollection<OfferApplication> OfferApplications { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    [InverseProperty("Offer")]
    public ICollection<Picture> Pictures { get; set; } = new List<Picture>();
}

public class OfferTypeLodging : Offer
{
    public string? LodgingType { get; set; }
    public string? AdditionalLodgingProperties { get; set; }
}

public class OfferTypeRequest : Offer {
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
    Closed, // the offer is not listed for users which aren't the owner. No new applications are possible but the offer can still be rewiewed
    Hidden // the offer is still in the database, but will not be listed anywhere.
}
