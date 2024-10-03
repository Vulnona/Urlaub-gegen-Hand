using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.ViewModels;

namespace UGH.Application.Offers;

public class GetOfferByIdQuery : IRequest<Result<OfferResponse>>
{
    public int OfferId { get; }

    public GetOfferByIdQuery(int offerId)
    {
        OfferId = offerId;
    }
}