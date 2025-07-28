#nullable enable
namespace UGHApi.Models;

public class SendCouponRequest
{
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }
}

public class CreateAndSendCouponRequest
{
    public string Email { get; set; }
    public int MembershipId { get; set; }
    public string? Name { get; set; }
}

public class SendExistingCouponRequest
{
    public int CouponId { get; set; }
}
