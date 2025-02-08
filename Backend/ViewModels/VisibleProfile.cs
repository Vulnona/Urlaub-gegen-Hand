namespace UGH.Domain.ViewModels;
// the Data other users are supposed to see when they
public class VisibleProfile
{    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] ProfilePicture { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    #nullable enable
    public string? FacebookLink { get; set; }
    public double? AverageRating { get; set; }
    public List<string>? Hobbies { get; set; }
    public List<string>? Skills { get; set; }
    #nullable disable
}
