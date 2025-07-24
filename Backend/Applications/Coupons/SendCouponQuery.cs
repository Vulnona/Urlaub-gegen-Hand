using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Coupons;

public class SendCouponQuery : IRequest<Result>
{
    public Guid UserId { get; }
    public string CouponCode { get; }

    public SendCouponQuery(Guid userId, string couponCode)
    {
        UserId = userId;
        CouponCode = couponCode;
    }
}
