using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UGHModels;

namespace UGHApi.Models
{
    public class Review
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }
        [Required]
        public int UserId { get; set; }   

        [ForeignKey("UserId")]
        public User User { get; set; }

        public reviewStatus Status { get; set; }  
    }

    public enum reviewStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
