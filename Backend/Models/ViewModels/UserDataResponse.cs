using UGH.Domain.Entities;

namespace UGH.Domain.ApplicationResponses;

public class UserDataResponse
{
    public Guid User_Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    
    // Geographic location data (NEW)
    #nullable enable
    public Address? Address { get; set; }
    #nullable restore
    
    public string Email_Address { get; set; }
    public bool IsEmailVerified { get; set; }
    #nullable enable
    public int? MembershipId { get; set; }
    #nullable restore
    public string Facebook_Link { get; set; }
    public string Link_RS { get; set; }
    public string Link_VS { get; set; }
    public int VerificationState { get; set; }
    public Membership? CurrentMembership { get; set; }
    public double AverageRating { get; set; }
}
