using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGH.Domain.Entities;

public class UserProfile
{
#pragma warning disable CS8632
    [Required]
    public int Id { get; set; }

    [Required]
    public Guid User_Id { get; set; }

    [ForeignKey("User_Id")]
    public User User { get; set; }
    public byte[]? UserPic { get; private set; }

    public ProfileOptions Options { get; set; }

    public string? Hobbies { get; set; } //Changed from Hobbies
    public string? Skills { get; set; } //Changed from Hobbies
    public string? Token { get; private set; }

    public void SetToken(string token)
    {
        Token = token;
    }

    public void SetUserProfilePicture(byte[] profilePicture)
    {
        UserPic = profilePicture;
    }
}

[Flags]
public enum ProfileOptions
{
    None = 0,
    Smoker = 1 << 0, // 1
    PetOwner = 1 << 1, // 2
    HaveLiabilityInsurance = 1 << 2 // 4
}
