using UGH.Domain.Entities;
using UGHApi.ViewModels.UserComponent;
namespace UGH.Domain.ViewModels;

public class OfferApplicationDto
{
#pragma warning disable CS8632
    public int OfferId { get; set; }
    public string OfferTitle { get; set; }
    public Guid HostId { get; set; }
    public UserC User { get; set; }
    public OfferApplicationStatus Status { get; set; }
    public String CreatedAt { get; set; }
    public bool HasReview { get; set; }
}
