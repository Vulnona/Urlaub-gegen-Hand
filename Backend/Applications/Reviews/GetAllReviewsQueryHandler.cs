using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace UGH.Application.Reviews;

public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, Result<List<Review>>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<GetAllReviewsQueryHandler> _logger;

    public GetAllReviewsQueryHandler(
        IReviewRepository reviewRepository,
        ILogger<GetAllReviewsQueryHandler> logger
    )
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<Result<List<Review>>> Handle(
        GetAllReviewsQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync();

            var reviewDtos = reviews
                .Select(
                    r =>
                        new Review
                        {
                            Id = r.Id,
                            OfferId = r.OfferId,
                            ReviewerId = r.ReviewerId,
                            ReviewedId = r.ReviewedId,
                            RatingValue = r.RatingValue,
                            ReviewComment = r.ReviewComment,
                            CreatedAt = r.CreatedAt
                        }
                )
                .ToList();

            return Result.Success(reviewDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<List<Review>>(
                Errors.General.InvalidOperation("Something went wrong while fetching the Reviews")
            );
        }
    }
}
