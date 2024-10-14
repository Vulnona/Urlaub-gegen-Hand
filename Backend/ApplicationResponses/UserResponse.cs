using UGH.Domain.Core;
using UGH.Domain.Entities;

namespace UGH.Domain.ApplicationResponses;

public class UserResponse
{
#pragma warning disable CS8632
    public Guid User_Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string PostCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string Email_Address { get; set; }
    public bool IsEmailVerified { get; set; }
    public int MembershipId { get; set; }
    public string? Facebook_link { get; set; }
    public string? Link_RS { get; set; }
    public string? Link_VS { get; set; }
    public string? Hobbies { get; set; }
    public string? Skills { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public string? About { get; set; }
    public UGH_Enums.VerificationState VerificationState { get; set; }
    public Membership CurrentMembership { get; set; }
    public double AverageRating { get; set; }
}
