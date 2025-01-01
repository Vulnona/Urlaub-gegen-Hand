using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGHApi.Shared;

namespace UGH.Application.Reviews;

public class GetAllReviewsQuery : IRequest<Result<PaginatedList<Review>>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetAllReviewsQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
