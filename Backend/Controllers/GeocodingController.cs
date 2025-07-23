#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using UGH.Domain.Services;
using UGH.Infrastructure.Services;

namespace UGHApi.Controllers;

[Route("api/geocoding")]
[ApiController]
[EnableCors]
public class GeocodingController : ControllerBase
{
    private readonly IGeocodingService _geocodingService;
    private readonly ILogger<GeocodingController> _logger;

    public GeocodingController(
        IGeocodingService geocodingService,
        ILogger<GeocodingController> logger)
    {
        _geocodingService = geocodingService;
        _logger = logger;
    }

    /// <summary>
    /// Search for addresses using OpenStreetMap/Nominatim
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> SearchAddresses(
        [FromQuery] string query,
        [FromQuery] string? countryCode = null,
        [FromQuery] int limit = 5)
    {
        try
        {
            // Add CORS headers manually
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "*");
            
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query parameter is required");

            var results = await _geocodingService.SearchAddressesAsync(query, countryCode, limit);
            
            _logger.LogInformation($"Address search for '{query}' returned {results.Count} results");
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error searching addresses for query: {query}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get address suggestions for autocomplete
    /// </summary>
    [HttpGet("suggestions")]
    public async Task<IActionResult> GetAddressSuggestions(
        [FromQuery] string query,
        [FromQuery] string? countryCode = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
                return BadRequest("Query must be at least 3 characters long");

            var suggestions = await _geocodingService.GetAddressSuggestionsAsync(query, countryCode);
            
            return Ok(suggestions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting address suggestions for query: {query}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Reverse geocoding - get address from coordinates
    /// </summary>
    [HttpGet("reverse")]
    public async Task<IActionResult> ReverseGeocode(
        [FromQuery] double lat,
        [FromQuery] double lon)
    {
        try
        {
            // Add CORS headers manually
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "*");
            
            if (lat < -90 || lat > 90)
                return BadRequest("Latitude must be between -90 and 90");
            
            if (lon < -180 || lon > 180)
                return BadRequest("Longitude must be between -180 and 180");

            var result = await _geocodingService.ReverseGeocodeAsync(lat, lon);
            
            if (result == null)
                return NotFound("No address found for the given coordinates");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error reverse geocoding lat:{lat}, lng:{lon}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Create and save a new address from geocoding data
    /// </summary>
    /*
    [HttpPost("create-address")]
    public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var address = await _addressService.CreateAddressAsync(
                request.Latitude,
                request.Longitude,
                request.DisplayName,
                request.HouseNumber,
                request.Road,
                request.City,
                request.Postcode,
                request.Country,
                request.CountryCode
            );

            _logger.LogInformation($"Created new address with ID {address.Id}: {address.DisplayName}");
            return Ok(address);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating address");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    */

    /// <summary>
    /// Find nearby addresses within a radius
    /// </summary>
    /*
    [HttpGet("nearby")]
    public async Task<IActionResult> FindNearbyAddresses(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm = 10)
    {
        try
        {
            if (latitude < -90 || latitude > 90)
                return BadRequest("Latitude must be between -90 and 90");
            
            if (longitude < -180 || longitude > 180)
                return BadRequest("Longitude must be between -180 and 180");

            if (radiusKm <= 0 || radiusKm > 100)
                return BadRequest("Radius must be between 0 and 100 km");

            var addresses = await _addressService.FindNearbyAddressesAsync(latitude, longitude, radiusKm);
            
            return Ok(addresses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error finding nearby addresses for lat:{latitude}, lng:{longitude}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    */
}

/// <summary>
/// Request model for creating a new address
/// </summary>
public class CreateAddressRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? HouseNumber { get; set; }
    public string? Road { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
}
