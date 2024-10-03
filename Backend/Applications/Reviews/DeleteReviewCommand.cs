using MediatR;
using UGH.Domain.Core;

public class DeleteReviewCommand : IRequest<Result>
{
    public int ReviewId { get; set; }
    public string Email { get; set; }

    public DeleteReviewCommand(int reviewId, string email)
    {
        ReviewId = reviewId;
        Email = email;
    }
}
