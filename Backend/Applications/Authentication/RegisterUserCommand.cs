using MediatR;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;

namespace UGH.Application.Authentication;

public class RegisterUserCommand : IRequest<Result<RegisterUserResponse>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string PostCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Email_Address { get; set; }
    public string Password { get; set; }
    public string Facebook_link { get; set; }
    public string Link_RS { get; set; }
    public string Link_VS { get; set; }
    public string State { get; set; }
}
