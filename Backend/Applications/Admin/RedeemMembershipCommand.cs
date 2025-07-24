using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Admin;

public class RedeemMembershipCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string CouponCode { get; set; }

    public RedeemMembershipCommand(Guid userId, string couponCode)
    {
        UserId = userId;
        CouponCode = couponCode;
    }
}
