using UGH.Domain.Entities;

namespace UGH.Domain.ViewModels;

public class OfferDTO
{
#pragma warning disable CS8632
    public int Id { get; set; }
    public byte[] ImageData { get; set; }
    public string Title { get; set; }
    public string? Accomodation { get; set; }
    public string? Accomodationsuitable { get; set; }
    public string Skills { get; set; }
    public Guid? HostId { get; set; }
    public string HostName { get; set; }
    public double AverageRating { get; set; }
    public string AppliedStatus { get; set; }
    public string Region { get; set; }
    public string Location { get; set; }

    public OfferDTO(Offer o, User u, OfferApplication oa){
        string appliedStatus = oa == null ? "CanApply" : oa.Status switch {
            OfferApplicationStatus.Pending => "Applied",
            OfferApplicationStatus.Approved => "Approved",
            OfferApplicationStatus.Rejected => "Rejected",
            _ => "Unknown",
        };
        Id = o.Id;
        ImageData = o.ImageData;
        Title = o.Title;
        Accomodation = o.Accomodation;
        Accomodationsuitable = o.accomodationsuitable;
        Skills = o.skills;
        HostId = o.HostId;
        HostName = $"{u.FirstName} {u.LastName}";
        AverageRating = o.AverageRating;
        Location = o.Location ?? "";
        Region = o.state ?? "";
        AppliedStatus = appliedStatus;
    }
}
