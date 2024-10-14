using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.ViewModels
{
    public class OfferViewModel
    {
#pragma warning disable CS8632
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Location { get; set; }
        public string Contact { get; set; }
        public string? Accommodation { get; set; }
        public string? AccommodationSuitable { get; set; }

        [Required]
        public string Skills { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
