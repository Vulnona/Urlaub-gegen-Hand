#nullable enable
using UGH.Domain.Entities;
using UGH.Domain.Services;

namespace UGH.Infrastructure.Services;

/// <summary>
/// Service for managing address data and integration with geocoding services
/// </summary>
public interface IAddressService
{
    /// <summary>
    /// Creates a new address from geographic coordinates and address data
    /// </summary>
    Task<Address> CreateAddressAsync(double latitude, double longitude, string displayName, 
        string? houseNumber = null, string? road = null, string? city = null, 
        string? postcode = null, string? country = null, string? countryCode = null);
    
    /// <summary>
    /// Updates an existing address
    /// </summary>
    Task<Address> UpdateAddressAsync(int addressId, double latitude, double longitude, string displayName,
        string? houseNumber = null, string? road = null, string? city = null, 
        string? postcode = null, string? country = null, string? countryCode = null);
    
    /// <summary>
    /// Gets address by ID
    /// </summary>
    Task<Address?> GetAddressByIdAsync(int addressId);
    
    /// <summary>
    /// Deletes an address if it's not referenced by any users or offers
    /// </summary>
    Task<bool> DeleteAddressAsync(int addressId);
    
    /// <summary>
    /// Finds addresses near a given location
    /// </summary>
    Task<List<Address>> FindNearbyAddressesAsync(double latitude, double longitude, double radiusKm = 10);
    
    /// <summary>
    /// Creates address from Nominatim search result
    /// </summary>
    Task<Address> CreateFromNominatimResultAsync(UGH.Domain.Services.AddressSearchResult result);
}
