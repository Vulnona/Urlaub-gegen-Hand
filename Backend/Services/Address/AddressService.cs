#nullable enable
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGHApi.DATA;
using UGH.Domain.Services;

namespace UGH.Infrastructure.Services;

/// <summary>
/// Implementation of address management services
/// </summary>
public class AddressService : IAddressService
{
    private readonly Ugh_Context _context;
    private readonly ILogger<AddressService> _logger;

    public AddressService(Ugh_Context context, ILogger<AddressService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Address> CreateAddressAsync(double latitude, double longitude, string displayName, 
        string? houseNumber = null, string? road = null, string? city = null, 
        string? postcode = null, string? country = null, string? countryCode = null)
    {
        try
        {
            var address = new Address
            {
                Latitude = latitude,
                Longitude = longitude,
                DisplayName = displayName,
                HouseNumber = houseNumber,
                Road = road,
                City = city,
                Postcode = postcode,
                Country = country,
                CountryCode = countryCode,
                CreatedAt = DateTime.UtcNow,
                Type = DetermineAddressType(city, road)
            };

            _context.addresses.Add(address);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Created new address with ID {address.Id}: {displayName}");
            return address;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating address: {displayName}");
            throw;
        }
    }

    public async Task<Address> UpdateAddressAsync(int addressId, double latitude, double longitude, string displayName,
        string? houseNumber = null, string? road = null, string? city = null, 
        string? postcode = null, string? country = null, string? countryCode = null)
    {
        try
        {
            var address = await _context.addresses.FindAsync(addressId);
            if (address == null)
                throw new ArgumentException($"Address with ID {addressId} not found");

            address.Latitude = latitude;
            address.Longitude = longitude;
            address.DisplayName = displayName;
            address.HouseNumber = houseNumber;
            address.Road = road;
            address.City = city;
            address.Postcode = postcode;
            address.Country = country;
            address.CountryCode = countryCode;
            address.UpdatedAt = DateTime.UtcNow;
            address.Type = DetermineAddressType(city, road);

            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Updated address with ID {address.Id}: {displayName}");
            return address;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating address with ID {addressId}");
            throw;
        }
    }

    public async Task<Address?> GetAddressByIdAsync(int addressId)
    {
        return await _context.addresses.FindAsync(addressId);
    }

    public async Task<bool> DeleteAddressAsync(int addressId)
    {
        try
        {
            var address = await _context.addresses
                .Include(a => a.Users)
                .Include(a => a.Offers)
                .FirstOrDefaultAsync(a => a.Id == addressId);

            if (address == null)
                return false;

            // Check if address is still referenced
            if (address.Users.Any() || address.Offers.Any())
            {
                _logger.LogWarning($"Cannot delete address {addressId} - still referenced by users or offers");
                return false;
            }

            _context.addresses.Remove(address);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Deleted address with ID {addressId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting address with ID {addressId}");
            throw;
        }
    }

    public async Task<List<Address>> FindNearbyAddressesAsync(double latitude, double longitude, double radiusKm = 10)
    {
        try
        {
            // Simple distance calculation using Haversine formula approximation
            var addresses = await _context.addresses
                .Where(a => Math.Abs(a.Latitude - latitude) <= radiusKm / 111.0 && // Rough degree approximation
                           Math.Abs(a.Longitude - longitude) <= radiusKm / (111.0 * Math.Cos(latitude * Math.PI / 180)))
                .ToListAsync();

            return addresses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error finding nearby addresses for lat:{latitude}, lng:{longitude}");
            throw;
        }
    }

    public async Task<Address> CreateFromNominatimResultAsync(UGH.Domain.Services.AddressSearchResult result)
    {
        return await CreateAddressAsync(
            result.Latitude,
            result.Longitude,
            result.DisplayName,
            result.HouseNumber,
            result.Road,
            result.City,
            result.Postcode,
            result.Country,
            result.CountryCode
        );
    }

    private static AddressType DetermineAddressType(string? city, string? road)
    {
        if (string.IsNullOrEmpty(city) && string.IsNullOrEmpty(road))
            return AddressType.Rural;
        
        if (!string.IsNullOrEmpty(city))
            return AddressType.Urban;
            
        return AddressType.Residential;
    }
}
