using MediatR;
using UGH.Domain.Core;
using UGHApi.ViewModels;

public class GetAllReviewsByUserIdQuery : IRequest<Result<List<ReviewDto>>>
{
    public Guid UserId { get; }

    public GetAllReviewsByUserIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
