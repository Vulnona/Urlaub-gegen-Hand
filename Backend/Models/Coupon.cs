using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Coupon code is required.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Coupon name is required.")]
        public string Name { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime StartDate { get; set; } // Start date of coupon validity
        public DateTime EndDate { get; set; }   // End date of coupon validity
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
