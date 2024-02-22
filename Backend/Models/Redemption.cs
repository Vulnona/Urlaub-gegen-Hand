namespace UGHApi.Models
{
    public class Redemption
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public int UserId { get; set; } // User ID who redeemed the coupon
        public DateTime RedeemedDate { get; set; }

        // Navigation property
        public Coupon Coupon { get; set; }
    }
}
