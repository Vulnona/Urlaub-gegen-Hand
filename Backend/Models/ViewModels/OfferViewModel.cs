using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.ViewModels
{
    public class OfferViewModel
    {
#pragma warning disable CS8632
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public string Contact { get; set; }
        public string? Accommodation { get; set; }
        public string? AccommodationSuitable { get; set; }

        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }

        [Required]
        public string Skills { get; set; }
        
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        [Required]
        public string DisplayName { get; set; } // Full formatted address
        public int? Id { get; set; } // optional, falls für Update benötigt
        public int? OfferId { get; set; } // für Controller-Kompatibilität
        // Multi-Bild-Unterstützung
        public IFormFile[] Images { get; set; }
        public string[] ExistingImages { get; set; }
        // modifications don't need a new image. If an image exists is checked in PutOffer
    }
}
