using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Coupons;

public class SendCouponQuery : IRequest<Result>
{
    public Guid? UserId { get; }
    public string? Email { get; }
    public string CouponCode { get; }

    public SendCouponQuery(Guid userId, string couponCode)
    {
        UserId = userId;
        Email = null;
        CouponCode = couponCode;
    }

    public SendCouponQuery(string email, string couponCode)
    {
        UserId = null;
        Email = email;
        CouponCode = couponCode;
    }
}
