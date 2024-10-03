namespace UGH.Domain.ApplicationResponses;

public class UserOffer
{
    public int Id { get; set; }
    public byte[] ImageData { get; set; }
    public string Title { get; set; }
    public string? Accomodation { get; set; }
    public string? Accomodationsuitable { get; set; }
    public string Skills { get; set; }
    public Guid HostId { get; set; }
    public double AverageRating { get; set; }
    public bool IsApplied { get; set; }
    public string Region { get; set; }
    public string Location { get; set; }
}
