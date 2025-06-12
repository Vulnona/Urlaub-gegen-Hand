namespace UGH.Domain.Entities;

public class Redemption
{
    public int Id { get; set; }
    public int CouponId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public DateTime RedeemedDate { get; set; }

    public Coupon Coupon { get; set; }
}
