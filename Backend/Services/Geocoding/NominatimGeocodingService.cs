#nullable enable
using System.Text.Json;
using UGH.Domain.Entities;
using UGH.Domain.Services;

namespace UGH.Infrastructure.Services;

/// <summary>
/// OpenStreetMap/Nominatim implementation for geocoding services
/// </summary>
public class NominatimGeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NominatimGeocodingService> _logger;
    private const string BaseUrl = "https://nominatim.openstreetmap.org";
    private const string UserAgent = "UGH-Platform/1.0 (contact@urlaub-gegen-hand.com)";

    public NominatimGeocodingService(HttpClient httpClient, ILogger<NominatimGeocodingService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        // Set required headers for Nominatim API
        _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
    }

    public async Task<List<AddressSearchResult>> SearchAddressesAsync(string query, string? countryCode = null, int limit = 5)
    {
        try
        {
            var url = $"{BaseUrl}/search?q={Uri.EscapeDataString(query)}&format=json&addressdetails=1&limit={limit}";
            
            if (!string.IsNullOrEmpty(countryCode))
            {
                url += $"&countrycodes={countryCode}";
            }

            _logger.LogInformation("Searching addresses for query: {Query}", query);
            
            var response = await _httpClient.GetStringAsync(url);
            var nominatimResults = JsonSerializer.Deserialize<List<NominatimSearchResult>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return nominatimResults?.Select(MapToAddressSearchResult).ToList() ?? new List<AddressSearchResult>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching addresses for query: {Query}", query);
            return new List<AddressSearchResult>();
        }
    }

    public async Task<AddressSearchResult?> ReverseGeocodeAsync(double latitude, double longitude)
    {
        try
        {
            var url = $"{BaseUrl}/reverse?lat={latitude}&lon={longitude}&format=json&addressdetails=1";
            
            _logger.LogInformation("Reverse geocoding for coordinates: {Lat}, {Lon}", latitude, longitude);
            
            var response = await _httpClient.GetStringAsync(url);
            _logger.LogInformation("Nominatim raw response: {Response}", response);
            
            var nominatimResult = JsonSerializer.Deserialize<NominatimSearchResult>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (nominatimResult != null)
            {
                var result = MapToAddressSearchResult(nominatimResult);
                _logger.LogInformation("Mapped result - DisplayName: '{DisplayName}', City: '{City}', Country: '{Country}'", 
                    result.DisplayName, result.City, result.Country);
                return result;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reverse geocoding coordinates: {Lat}, {Lon}", latitude, longitude);
            return null;
        }
    }

    public async Task<AddressSearchResult?> ValidateAddressAsync(double latitude, double longitude)
    {
        // For validation, we use reverse geocoding to check if coordinates are valid
        return await ReverseGeocodeAsync(latitude, longitude);
    }

    public async Task<List<AddressSuggestion>> GetAddressSuggestionsAsync(string query, string? countryCode = null)
    {
        var searchResults = await SearchAddressesAsync(query, countryCode, 10);
        
        return searchResults.Select(result => new AddressSuggestion
        {
            DisplayName = result.DisplayName,
            Latitude = result.Latitude,
            Longitude = result.Longitude,
            City = result.City,
            Country = result.Country,
            Importance = result.Importance
        }).ToList();
    }

    private static AddressSearchResult MapToAddressSearchResult(NominatimSearchResult nominatimResult)
    {
        var city = nominatimResult.Address?.City ?? nominatimResult.Address?.Town ?? nominatimResult.Address?.Village;
        var country = nominatimResult.Address?.Country;
        var road = nominatimResult.Address?.Road;
        var houseNumber = nominatimResult.Address?.HouseNumber;
        var postcode = nominatimResult.Address?.Postcode;
        
        // Create a fallback displayName if Nominatim doesn't provide one
        var displayName = nominatimResult.DisplayName;
        if (string.IsNullOrEmpty(displayName))
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(houseNumber)) parts.Add(houseNumber);
            if (!string.IsNullOrEmpty(road)) parts.Add(road);
            if (!string.IsNullOrEmpty(city)) parts.Add(city);
            if (!string.IsNullOrEmpty(postcode)) parts.Add(postcode);
            if (!string.IsNullOrEmpty(country)) parts.Add(country);
            
            displayName = parts.Count > 0 ? string.Join(", ", parts) : $"Location ({nominatimResult.Lat}, {nominatimResult.Lon})";
        }
        
        return new AddressSearchResult
        {
            Latitude = double.Parse(nominatimResult.Lat ?? "0"),
            Longitude = double.Parse(nominatimResult.Lon ?? "0"),
            DisplayName = displayName,
            HouseNumber = houseNumber,
            Road = road,
            Suburb = nominatimResult.Address?.Suburb,
            City = city,
            County = nominatimResult.Address?.County,
            State = nominatimResult.Address?.State,
            Postcode = postcode,
            Country = country,
            CountryCode = nominatimResult.Address?.CountryCode,
            OsmId = nominatimResult.OsmId,
            OsmType = nominatimResult.OsmType,
            PlaceId = nominatimResult.PlaceId,
            Type = DetermineAddressType(nominatimResult.Type, nominatimResult.Class),
            Importance = nominatimResult.Importance
        };
    }

    private static AddressType DetermineAddressType(string? type, string? @class)
    {
        return (@class?.ToLower(), type?.ToLower()) switch
        {
            ("building", "residential") => AddressType.Residential,
            ("building", "commercial") => AddressType.Commercial,
            ("tourism", _) => AddressType.Tourism,
            ("landuse", "residential") => AddressType.Residential,
            ("landuse", "commercial") => AddressType.Commercial,
            ("landuse", "industrial") => AddressType.Commercial,
            ("place", "city") => AddressType.Urban,
            ("place", "town") => AddressType.Urban,
            ("place", "village") => AddressType.Rural,
            _ => AddressType.Residential
        };
    }
}

/// <summary>
/// Nominatim API response structure
/// </summary>
internal class NominatimSearchResult
{
    public string? PlaceId { get; set; }
    public string? Licence { get; set; }
    public string? OsmType { get; set; }
    public long? OsmId { get; set; }
    public string? Lat { get; set; }
    public string? Lon { get; set; }
    public string? Class { get; set; }
    public string? Type { get; set; }
    public double? Importance { get; set; }
    public string? DisplayName { get; set; }
    public NominatimAddress? Address { get; set; }
}

internal class NominatimAddress
{
    public string? HouseNumber { get; set; }
    public string? Road { get; set; }
    public string? Suburb { get; set; }
    public string? City { get; set; }
    public string? Town { get; set; }
    public string? Village { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
}
