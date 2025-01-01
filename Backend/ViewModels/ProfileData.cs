using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.ViewModels;

public class ProfileData
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

    [Required]
    public string Street { get; set; }

    [Required]
    public string HouseNumber { get; set; }

    [Required]
    public string PostCode { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }

    [Required]
    public string Country { get; set; }

    public string? Hobbies { get; set; }
    public string? Skills { get; set; }
}
