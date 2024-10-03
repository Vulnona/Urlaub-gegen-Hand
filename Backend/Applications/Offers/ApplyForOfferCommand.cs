using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Offers;

public class ApplyForOfferCommand : IRequest<Result>
{
    public int OfferId { get; }
    public Guid UserId { get; }

    public ApplyForOfferCommand(int offerId, Guid userId)
    {
        OfferId = offerId;
        UserId = userId;
    }
}
