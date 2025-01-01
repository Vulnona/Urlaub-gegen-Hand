using MediatR;
using UGH.Domain.Core;
using UGHApi.Shared;
using UGHApi.ViewModels;

public class GetAllReviewsByUserIdQuery : IRequest<Result<PaginatedList<ReviewDto>>>
{
    public Guid UserId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetAllReviewsByUserIdQuery(Guid userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
