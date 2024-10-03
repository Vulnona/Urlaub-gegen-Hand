using MediatR;
using UGH.Contracts.Review;
using UGH.Domain.Core;

namespace UGH.Application.Reviews;

public record UpdateReviewCommand(UpdateReviewRequest updateReviewRequest, Guid UserId)
    : IRequest<Result>;
