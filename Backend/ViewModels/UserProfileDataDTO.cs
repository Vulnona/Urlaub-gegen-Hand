namespace UGH.Domain.ViewModels;

public class UserProfileDataDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] ProfilePicture { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string PostCode { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string? FacebookLink { get; set; }
    public string? Link_RS { get; set; }
    public string? Link_VS { get; set; }
    public string? VerificationState { get; set; }
    public double? UserRating { get; set; }
    public List<string>? Hobbies { get; set; }
    public List<string>? Skills { get; set; }
}
