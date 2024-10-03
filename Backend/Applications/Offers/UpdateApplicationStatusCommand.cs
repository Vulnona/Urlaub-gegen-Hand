using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Offers;

public class UpdateApplicationStatusCommand : IRequest<Result>
{
    public int OfferId { get; }
    public Guid UserId { get; }
    public Guid HostId { get; }
    public bool IsApprove { get; }

    public UpdateApplicationStatusCommand(int offerId, Guid userId, Guid hostId, bool isApprove)
    {
        OfferId = offerId;
        UserId = userId;
        HostId = hostId;
        IsApprove = isApprove;
    }
}
