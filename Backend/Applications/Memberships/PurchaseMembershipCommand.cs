using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Memberships;

public class PurchaseMembershipCommand : IRequest<Result<bool>>
{
    public Guid UserId { get; set; }
    public int MembershipId { get; set; }

    public PurchaseMembershipCommand(Guid userId, int membershipId)
    {
        UserId = userId;
        MembershipId = membershipId;
    }
}
