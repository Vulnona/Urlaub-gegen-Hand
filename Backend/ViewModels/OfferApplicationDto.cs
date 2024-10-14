using UGH.Domain.Entities;

namespace UGH.Domain.ViewModels;

public class OfferApplicationDto
{
#pragma warning disable CS8632
    public int OfferId { get; set; }
    public Guid HostId { get; set; }
    public OfferDto Offer { get; set; }
    public UserDto User { get; set; }
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

public class UserDto
{
    public Guid User_Id { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Facebook_link { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Gender { get; set; }
}
