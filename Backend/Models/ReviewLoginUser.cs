using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UGHModels;

namespace UGHApi.Models
{
    public class ReviewLoginUser
    {
        public int Id { get; set; }

        [Required]
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }


        public string AddReviewForLoginUser { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public bool IsReviewPeriodOver => CreatedAt.Value.AddDays(14) <= DateTime.Now;
    }
}