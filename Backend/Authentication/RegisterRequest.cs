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

    [Required]
    public string Street { get; set; }

    [Required]
    public string HouseNumber { get; set; }

    [Required]
    public string PostCode { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public string State { get; set; }
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
