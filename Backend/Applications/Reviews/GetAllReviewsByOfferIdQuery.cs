using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Reviews;

public class GetAllReviewsByOfferIdQuery : IRequest<Result<List<ReviewDto>>>
{
    public int OfferId { get; }

    public GetAllReviewsByOfferIdQuery(int offerId)
    {
        OfferId = offerId;
    }
}
