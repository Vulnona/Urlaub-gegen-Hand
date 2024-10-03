using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Offers;

public class DeleteOfferCommand : IRequest<Result>
{
    public int OfferId { get; }

    public DeleteOfferCommand(int offerId)
    {
        OfferId = offerId;
    }
}
