using MediatR;
using UGH.Domain.Core;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Reviews;

public class GetAllReviewsByOfferIdQuery : IRequest<Result<PaginatedList<ReviewDto>>>
{
    public int OfferId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetAllReviewsByOfferIdQuery(int offerId, int pageNumber, int pageSize)
    {
        OfferId = offerId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
