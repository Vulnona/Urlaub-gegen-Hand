using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.ViewModels;

// used for update-user-data
public class UserData
{
#pragma warning disable CS8632
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; }
    
    
    // Geographic location data 
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? DisplayName { get; set; } // Full formatted address
    public int? Id { get; set; }
    public string? Hobbies { get; set; }
    public string? Skills { get; set; }
}
