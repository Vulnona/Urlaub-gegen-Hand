using System;
using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    private string _VerificationURL=string.Empty;
  
    [EmailAddress]

    [Required]
    public string Email_Address { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string DateOfBirth { get; set; }

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
    public string State { get; set; }
    public string Facebook_link { get; set; }
    public string Link_RS { get; set; }
    public string Link_VS{ get; set; }

    //[Required]
    // ATTENTION: URL needs to contain placeholders '*USER_ID*' for User_ID and '*TOKEN*' for VerificationToken !
    //public string VerificationURL
    //{
    //    get
    //        {
    //            return _VerificationURL;
    //        } 

    //    set
    //    {
    //        if (!( (value.ToUpper().Contains("*USER_ID*"))&& (value.ToUpper().Contains("*TOKEN*"))))
    //        {
    //            throw (new InvalidDataException("URL needs to contain placeholders '*USER_ID*' for User_ID and '*TOKEN*' for VerificationToken !"));
    //        }
    //        else
    //        {
    //            _VerificationURL=value;
    //        }
    //    }

    //}



}

public class LoginModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
public class RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
}
public class ResendEmailVerification
{
    public required string Email { get; set; }
}

