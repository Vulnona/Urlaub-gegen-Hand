using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UGHModels;

namespace UGHApi.Models
{
    public class UserProfile
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int User_Id { get; set; }

        [ForeignKey("User_Id")]
        public User User { get; set; }

        public byte[]? UserPic { get; set; }

        public ProfileOptions Options { get; set; }

        public string Hobbies { get; set; }
        public string Token { get; internal set; }

    }

    [Flags]
    public enum ProfileOptions
    {
        None = 0,
        Smoker = 1 << 0, // 1
        PetOwner = 1 << 1, // 2
        HaveLiabilityInsurance = 1 << 2 // 4
    }
}
