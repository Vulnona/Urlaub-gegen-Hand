using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UGH.Domain.Core;

namespace UGH.Domain.Entities;

public class User
{
    [Key]
    public Guid User_Id { get; set; }

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
    public string Country { get; set; }

    [Required]
    public string State { get; set; }

    [Required, EmailAddress]
    public string Email_Address { get; set; }

    [Required]
    public string Password { get; set; }

    public string? SaltKey { get; set; }

    public bool IsEmailVerified { get; set; }
    public int MembershipId { get; set; }
    public string? Facebook_link { get; set; }
    public string? Link_RS { get; set; }
    public string? Link_VS { get; set; }

    public string? Hobbies { get; set; }
    public string? Skills { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public string? About { get; set; }

    [Required]
    public UGH_Enums.VerificationState VerificationState { get; set; }

    [ForeignKey("MembershipId")]
    public Membership CurrentMembership { get; set; }

    public ICollection<Offer> Offers { get; set; } = new List<Offer>();

    private UserProfile Profile { get; set; }

    public double AverageRating
    {
        get
        {
            if (Offers == null || !Offers.Any())
            {
                return 0.0;
            }

            var totalRatings = Offers
                .SelectMany(o => o.Reviews)
                .Select(r => r.RatingValue)
                .ToList();

            return totalRatings.Any() ? totalRatings.Average() : 0.0;
        }
    }

    public void SetProfilePicture(byte[] picture)
    {
        ProfilePicture = picture;
    }

    public void SetVerifyStatus(bool status)
    {
        IsEmailVerified = status;
    }

    public void SetMembershipId(int membershipId)
    {
        MembershipId = membershipId;
    }

    public User() { }

    public User(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string gender,
        string street,
        string houseNumber,
        string postCode,
        string city,
        string country,
        string emailAddress,
        bool isEmailVerified,
        string password,
        string saltKey,
        string facebook_link,
        string link_RS,
        string link_VS,
        string state
    )
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Street = street;
        HouseNumber = houseNumber;
        PostCode = postCode;
        City = city;
        Country = country;
        Email_Address = emailAddress;
        IsEmailVerified = isEmailVerified;
        Password = password;
        SaltKey = saltKey;
        VerificationState = UGH_Enums.VerificationState.IsNew;
        Facebook_link = facebook_link;
        Link_RS = link_RS;
        Link_VS = link_VS;
        State = state;
    }
}
