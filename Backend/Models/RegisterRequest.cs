using System;
using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    private string _VerificationURL=string.Empty;
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public int User_Id { get; set; }

    [Required]
    public string VisibleName { get; set; }

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

    [Required]
    public string Email_Adress { get; set; }

    [Required]
    // ATTENTION: URL needs to contain placeholders 'USER_ID' for User_ID and 'TOKEN' for VerificationToken !
    public string VerificationURL
    {
        get
            {
                return _VerificationURL;
            } 
        
        set
        {
            if (!( (value.ToUpper().Contains("USER_ID"))&& (value.ToUpper().Contains("TOKEN"))))
            {
                throw (new InvalidDataException("URL needs to contain placeholders 'USER_ID' for User_ID and 'TOKEN' for VerificationToken !"));
            }
            else
            {
                _VerificationURL=value;
            }
        }
        
    }

    
}
