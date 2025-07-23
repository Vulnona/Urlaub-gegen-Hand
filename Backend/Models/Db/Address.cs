using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGH.Domain.Entities;

/// <summary>
/// Geographic address model using OpenStreetMap/Nominatim data structure
/// This replaces the old separate Street, City, State, Country fields
/// </summary>
public class Address
{
    [Key]
    public int Id { get; set; }

    // Geographic coordinates (required for map functionality)
    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    // Structured address components from Nominatim
    [Required]
    public string DisplayName { get; set; } // Full formatted address

    // Detailed address components
    public string? HouseNumber { get; set; }
    public string? Road { get; set; } // Street name
    public string? Suburb { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; } // ISO country code (e.g., "DE", "AT", "CH")

    // OpenStreetMap metadata
    public long? OsmId { get; set; } // OpenStreetMap ID for reference
    public string? OsmType { get; set; } // node, way, relation
    public string? PlaceId { get; set; } // Nominatim place ID
    
    // Address type classification
    public AddressType Type { get; set; } = AddressType.Residential;

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}

public enum AddressType
{
    Residential,
    Commercial,
    Tourism,
    Rural,
    Urban,
    Other
}
