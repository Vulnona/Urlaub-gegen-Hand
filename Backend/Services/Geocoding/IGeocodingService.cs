#nullable enable
using UGH.Domain.Entities;

namespace UGH.Domain.Services;

/// <summary>
/// Interface for geographic and address services using OpenStreetMap/Nominatim
/// </summary>
public interface IGeocodingService
{
    /// <summary>
    /// Search for addresses based on query string
    /// </summary>
    Task<List<AddressSearchResult>> SearchAddressesAsync(string query, string? countryCode = null, int limit = 5);

    /// <summary>
    /// Get detailed address information from coordinates
    /// </summary>
    Task<AddressSearchResult?> ReverseGeocodeAsync(double latitude, double longitude);

    /// <summary>
    /// Validate and format address coordinates
    /// </summary>
    Task<AddressSearchResult?> ValidateAddressAsync(double latitude, double longitude);

    /// <summary>
    /// Get suggestions while user types (autocomplete)
    /// </summary>
    Task<List<AddressSuggestion>> GetAddressSuggestionsAsync(string query, string? countryCode = null);
}

/// <summary>
/// Result from geocoding service
/// </summary>
public class AddressSearchResult
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? HouseNumber { get; set; }
    public string? Road { get; set; }
    public string? Suburb { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public long? OsmId { get; set; }
    public string? OsmType { get; set; }
    public string? PlaceId { get; set; }
    public AddressType Type { get; set; } = AddressType.Residential;
    public double? Importance { get; set; } // Nominatim importance score
}

/// <summary>
/// Address suggestion for autocomplete
/// </summary>
public class AddressSuggestion
{
    public string DisplayName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public double? Importance { get; set; }
}
