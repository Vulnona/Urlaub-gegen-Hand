using UGH.Domain.Entities;
using UGHApi.ViewModels.UserComponent;
namespace UGH.Domain.ViewModels;

public class OfferApplicationDto
{
#pragma warning disable CS8632
    public int OfferId { get; set; }
    public Guid HostId { get; set; }
    public OfferDto Offer { get; set; }
    public UserC User { get; set; }
    public OfferApplicationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class OfferDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public byte[] ImageData { get; set; }
    public string ImageMimeType { get; set; }
}
