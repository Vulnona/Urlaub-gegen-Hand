namespace UGH.Application.Offers.DTOs;

public class OfferDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Skills { get; set; }
    public string State { get; set; }
    public Guid HostId { get; set; }
    public string HostName { get; set; }
    public int NumberOfApplications { get; set; }
    public bool HasApplied { get; set; }
}
