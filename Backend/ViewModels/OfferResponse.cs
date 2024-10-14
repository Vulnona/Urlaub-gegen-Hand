using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGH.Domain.ViewModels;

public class OfferResponse
{
#pragma warning disable CS8632
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Location { get; set; }
    public byte[] ImageData { get; set; }
    public string ImageMimeType { get; set; }
    public string? Contact { get; set; }
    public string? Accomodation { get; set; }
    public string? AccomodationSuitable { get; set; }
    public string Skills { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public double AverageRating { get; set; }
    public UserResponse User { get; set; }
}

public class UserResponse
{
    public Guid User_Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
