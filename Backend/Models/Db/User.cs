#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UGH.Domain.Core;
using UGHApi.Entities;

namespace UGH.Domain.Entities
{

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

        public int? AddressId { get; set; }
        
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        [Required, EmailAddress]
        public string Email_Address { get; set; }

        [Required]
        public string Password { get; set; }
        public string? SaltKey { get; set; }

        // 2FA Properties
        public bool IsTwoFactorEnabled { get; set; } = false;
        public string? TwoFactorSecret { get; set; }
        public string? BackupCodes { get; set; } // JSON array of backup codes
        
        // Brute Force Protection
        public int FailedBackupCodeAttempts { get; set; } = 0;
        public DateTime? LastFailedBackupCodeAttempt { get; set; }
        public bool IsBackupCodeLocked { get; set; } = false;

        public bool IsEmailVerified { get; set; }
        public int? MembershipId { get; set; }
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
        public UserRoles UserRole { get; set; }
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();

        public UserProfile UserProfile { get; set; }
        // Reviews written by this user
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        // Reviews received by this user
        public ICollection<Review> ReceivedReviews { get; set; } = new List<Review>();

        public void SetProfilePicture(byte[] picture)
        {
            ProfilePicture = picture;
        }

        public void SetVerifyStatus(bool status)
        {
            IsEmailVerified = status;
        }

        public void SetMembershipId(int? membershipId)
        {
            MembershipId = membershipId;
        }

        public User() { }

        public User(
            string firstName,
            string lastName,
            DateOnly dateOfBirth,
            string gender,
            int? addressId,
            string emailAddress,
            bool isEmailVerified,
            string password,
            string saltKey,
            string facebook_link,
            string link_RS,
            string link_VS
        )
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            AddressId = addressId;
            Email_Address = emailAddress;
            IsEmailVerified = isEmailVerified;
            Password = password;
            SaltKey = saltKey;
            VerificationState = UGH_Enums.VerificationState.IsNew;
            Facebook_link = facebook_link;
            Link_RS = link_RS;
            Link_VS = link_VS;
        }
    }
}
    public enum UserRoles {
        User,
        Admin
    }
