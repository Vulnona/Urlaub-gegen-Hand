namespace UGH.Domain.ViewModels;
// the data other users are supposed to see when they click on another user
public class VisibleProfile
{
    public Guid UserId {get; set;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] ProfilePicture { get; set; }
    public int Age { get; set; }
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
    // if this flag is true more privileged information can be shared
    public bool contact { get; set; }
}
