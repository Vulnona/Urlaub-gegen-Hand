using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Coupons;

public class AddCouponCommand : IRequest<Result>
{
    public Guid UserId { get; }
    public int MembershipId { get; }

    public AddCouponCommand(Guid userId, int membershipId)
    {
        UserId = userId;
        MembershipId = membershipId;

    }
};

// using MediatR;
// using UGH.Domain.Core;

// namespace UGHApi.Applications.Coupons;

// public class AddCouponCommand : IRequest<Result>
// {
//     public Guid UserId { get; }

//     public AddCouponCommand(Guid userId)
//     {
//         UserId = userId;
//     }
// };
