using System.ComponentModel.DataAnnotations;

namespace UGH.Contracts.Authentication;

#pragma warning disable CS8632
public class RegisterRequest
{
    private string _VerificationURL = string.Empty;

    [EmailAddress]
    [Required]
    public string Email_Address { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; }

    // Geographic location data (NEW) - replacing old address fields
    [Required]
    public double Latitude { get; set; }
    
    [Required]
    public double Longitude { get; set; }
    
    [Required]
    public string DisplayName { get; set; } // Full formatted address from map selection
    public int? Id { get; set; } // optional, falls für Update benötigt
    
    public string? Facebook_link { get; set; }
    public string Link_RS { get; set; }
    public string Link_VS { get; set; }
}

public class LoginModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
}

public class ResendEmailVerification
{
    public required string Email { get; set; }
}
