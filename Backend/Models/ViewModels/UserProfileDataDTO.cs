using UGH.Domain.Entities;

namespace UGH.Domain.ViewModels;

public class UserProfileDataDTO
{
#pragma warning disable CS8632
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] ProfilePicture { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }
    
    // Geographic location data (flach, ohne Navigation Properties)
    public AddressDTO? Address { get; set; }
    
    public string? FacebookLink { get; set; }
    public string? Link_RS { get; set; }
    public string? Link_VS { get; set; }
    public string? VerificationState { get; set; }
    public double? UserRating { get; set; }
    public List<string>? Hobbies { get; set; }
    public List<object>? Skills { get; set; }
    public DateTime? MembershipEndDate { get; set; }
}
