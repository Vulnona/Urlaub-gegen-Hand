using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGHApi.Models
{
    public class ReviewPost
    {
        public int Id { get; set; }

        public int ReviewOfferUserId { get; set; }
        [ForeignKey("ReviewOfferUserId")]
        public ReviewOfferUser ReviewOfferUser { get; set; }


        public int ReviewLoginUserId { get; set; }
        [ForeignKey("ReviewLoginUserId")]
        public ReviewLoginUser ReviewLoginUser { get; set; }

        public string OfferUserReviewPost { get; set; }
        public string LoginUserReviewPost { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}