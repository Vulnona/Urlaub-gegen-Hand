using System.ComponentModel.DataAnnotations.Schema;
using UGHModels;

namespace UGHApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }

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
