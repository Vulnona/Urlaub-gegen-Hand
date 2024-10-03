namespace UGH.Domain.ViewModels;

public class OfferDTO
{
    public int Id { get; set; }
    public byte[] ImageData { get; set; }
    public string Title { get; set; }
    public string? Accomodation { get; set; }
    public string? Accomodationsuitable { get; set; }
    public string Skills { get; set; }
    public Guid HostId { get; set; }
    public string HostName { get; set; }
    public double AverageRating { get; set; }
    public string AppliedStatus { get; set; }
    public string Region { get; set; }
    public string Location { get; set; }
}
