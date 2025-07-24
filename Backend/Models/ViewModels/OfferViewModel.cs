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
        public string Contact { get; set; }
        public string? Accommodation { get; set; }
        public string? AccommodationSuitable { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        [Required]
        public string Skills { get; set; }
        
        // Geographic location data (NEW) - replacing old Country/State/City fields
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        [Required]
        public string DisplayName { get; set; } // Full formatted address
        
        // Optional location components
        public string? City { get; set; }
        public string? Country { get; set; }

        public int OfferId { get; set; }
        // modifications don't need a new image. If an image exists is checked in PutOffer
        public IFormFile Image { get; set; }
    }
}
