using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using UGHModels;

namespace UGHApi.Models
{
    public class Offer
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ImageMimeType { get; set; }
        public string Contact { get; set; }
        public string Accomodation { get; set; }
        public string accomodationsuitable { get; set; }
        public string skills { get; set; }
        public int Region_ID { get; set; }
        [ForeignKey("Region_ID")]
        public Region Region { get; set; }
        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
        public enum Fb_Status { pending , posted }
    }
}