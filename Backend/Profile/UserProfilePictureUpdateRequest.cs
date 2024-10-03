using System.ComponentModel.DataAnnotations;

namespace UGH.Contracts.Profile;

public class UserProfilePictureUpdateRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    public byte[] ProfilePic { get; set; }
}
