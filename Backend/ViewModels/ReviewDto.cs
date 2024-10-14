using UGH.Domain.Entities;

namespace UGHApi.ViewModels;

public class ReviewDto
{
#pragma warning disable CS8632
    public int Id { get; set; }
    public int OfferId { get; set; }
    public int RatingValue { get; set; }
    public string? ReviewComment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserDto Reviewer { get; set; }
    public UserDto Reviewed { get; set; }
    public Offer Offer { get; set; }
}

public class UserDto
{
    public Guid User_Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[]? ProfilePicture { get; set; }
}
