using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Reviews;
#pragma warning disable CS8632
public record AddReviewCommand(
    int OfferId,
    int RatingValue,
    string? ReviewComment,
    Guid UserId,
    Guid? ReviewedUserId = null
) : IRequest<Result>;
