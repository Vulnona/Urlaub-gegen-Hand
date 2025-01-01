using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Admin;

public class RedeemMembershipCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public int MembershipId { get; set; } = 1;
    public string CouponCode { get; set; }

    public RedeemMembershipCommand(Guid userId, int membershipId, string couponCode)
    {
        UserId = userId;
        MembershipId = membershipId;
        CouponCode = couponCode;
    }
}
