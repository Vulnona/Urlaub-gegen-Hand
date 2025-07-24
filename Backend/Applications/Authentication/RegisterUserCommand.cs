using MediatR;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;

namespace UGH.Application.Authentication;

public class RegisterUserCommand : IRequest<Result<RegisterUserResponse>>
{
#pragma warning disable CS8632
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Gender { get; set; }
    
    // Address data for creating Address entity
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string DisplayName { get; set; }
    public string? HouseNumber { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    
    public string Email_Address { get; set; }
    public string Password { get; set; }
    public string Facebook_link { get; set; }
    public string Link_RS { get; set; }
    public string Link_VS { get; set; }
}
