using System.ComponentModel.DataAnnotations;
using NuGet.Protocol.Plugins;
using UGHApi.Models;

namespace UGHModels{

    public class User{

        
        
        
        
        public User(int user_Id, string visibleName, string firstName, string lastName, DateOnly dateOfBirth, string gender, string street, string houseNumber, string postCode, string city, string country, string email_Adress, bool isEmailVerified) 
        {
            this.User_Id = user_Id;
                this.VisibleName = visibleName;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.DateOfBirth = dateOfBirth;
                this.Gender = gender;
                this.Street = street;
                this.HouseNumber = houseNumber;
                this.PostCode = postCode;
                this.City = city;
                this.Country = country;
                this.Email_Adress = email_Adress;
                this.IsEmailVerified = isEmailVerified;
                
               
        }

        public User( string visibleName, string firstName, string lastName, DateOnly dateOfBirth, string gender, string street, string houseNumber, string postCode, string city, string country, string email_Adress, bool isEmailVerified,string password,string saltKey) 
        {
                this.VisibleName = visibleName;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.DateOfBirth = dateOfBirth;
                this.Gender = gender;
                this.Street = street;
                this.HouseNumber = houseNumber;
                this.PostCode = postCode;
                this.City = city;
                this.Country = country;
                this.Email_Adress = email_Adress;
                this.IsEmailVerified = isEmailVerified;
                this.Password = password;
            this.SaltKey = saltKey;
                
               
        }
        [Key]
        public int User_Id {get;set;}
        [Required]
        public string Password { get;set;}
        public string SaltKey {  get;set;}
        [Required]
        public string VisibleName{get;set;}
        [Required]
        public string FirstName {get;set;}
        [Required]
        public string LastName{get;set;}
        [Required]
        public DateOnly DateOfBirth{get;set;}
        public string Gender{get;set;}
        [Required]
        public string Street{get;set;}
        [Required]
        public string HouseNumber{get;set;}
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string City{get;set;}
        [Required]
        public string Country{get;set;}
        [Required]
        public string Email_Adress {get;set;}
        public bool IsEmailVerified{get;set;}
        public string? PasswordHash{get;}
        [Required]
        public UGH_Enums.VerificationState VerificationState{get;set;}
        public Membership? CurrentMembership{get;set;}
//		public string FacebookProfil { get; set; }
    }
}