using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class OfferViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Accommodation { get; set; }
        public string AccommodationSuitable { get; set; }
        public string Skills { get; set; }
        public int Skill_ID { get; set; }
        //public int Region_ID { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public int User_Id { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
