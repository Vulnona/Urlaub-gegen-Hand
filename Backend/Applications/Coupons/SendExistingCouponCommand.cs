using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Coupons;

public class SendExistingCouponCommand : IRequest<Result>
{
    public int CouponId { get; }

    public SendExistingCouponCommand(int couponId)
    {
        CouponId = couponId;
    }
} 