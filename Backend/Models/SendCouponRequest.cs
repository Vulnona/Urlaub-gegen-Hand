namespace UGHApi.Models;

public class SendCouponRequest
{
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }
}
