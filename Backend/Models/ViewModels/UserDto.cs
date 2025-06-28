using UGH.Domain.Core;

namespace UGHApi.ViewModels;

public class UserDTO
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
    public int? MembershipId { get; set; }
    public string? Facebook_link { get; set; }
    public string? Link_RS { get; set; }
    public string? Link_VS { get; set; }
    public List<string?> Hobbies { get; set; } = new List<string?>();
    public List<string?> Skills { get; set; } = new List<string?>();
    public byte[]? ProfilePicture { get; set; }
    public string? About { get; set; }
    public UGH_Enums.VerificationState VerificationState { get; set; }
    public double AverageRating { get; set; }
    public DateTime? MembershipEndDate { get; set; }
}
