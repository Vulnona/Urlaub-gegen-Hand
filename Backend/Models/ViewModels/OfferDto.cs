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
    public string Description {get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
    public byte[] HostPicture { get; set; }
    public OfferDTO(OfferTypeLodging o, User u, OfferApplication oa){
        string appliedStatus = oa == null ? "CanApply" : oa.Status switch {
            OfferApplicationStatus.Pending => "Applied",
            OfferApplicationStatus.Approved => "Approved",
            OfferApplicationStatus.Rejected => "Rejected",
            _ => "Unknown",
        };
        Id = o.Id;
        ImageData = o.Picture.ImageData;
        Title = o.Title;
        Accomodation = o.AdditionalLodgingProperties;
        Accomodationsuitable = o.Requirements;
        Skills = o.Skills;
        HostId = o.UserId;
        HostName = $"{u.FirstName} {u.LastName}";
        AverageRating = 0;
        Location = o.Location ?? "";
        Region = "";
        AppliedStatus = appliedStatus;
        Description = o.Description;
        FromDate = o.FromDate.ToString("dd.MM.yyyy");
        ToDate = o.ToDate.ToString("dd.MM.yyyy");
        HostPicture = u.ProfilePicture;
    }
}
