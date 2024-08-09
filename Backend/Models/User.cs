using System;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;

namespace UGHModels
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }

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

        [Required]
        public string Email_Address { get; set; }

        [Required]
        public string Password { get; set; }

        public string SaltKey { get; set; }

        public bool IsEmailVerified { get; set; }
        public string Facebook_link { get; set; }
        public string Link_RS { get; set; }
        public string Link_VS { get; set; }

        [Required]
        public UGH_Enums.VerificationState VerificationState { get; set; }

        public Membership CurrentMembership { get; set; }

        public User()
        {
        }

        public User(string firstName, string lastName, DateOnly dateOfBirth, string gender, string street, string houseNumber,
            string postCode, string city, string country, string emailAddress, bool isEmailVerified, string password,
            string saltKey, string facebook_link, string link_RS, string link_VS, string state)
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
}
