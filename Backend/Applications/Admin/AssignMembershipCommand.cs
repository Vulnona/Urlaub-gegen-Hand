using MediatR;

namespace UGHApi.Applications.Admin;

public class AssignMembershipCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public int MembershipId { get; set; }

    public AssignMembershipCommand(Guid userId, int membershipId)
    {
        UserId = userId;
        MembershipId = membershipId;
    }
}
