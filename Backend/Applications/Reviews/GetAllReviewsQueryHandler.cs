using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Shared;

namespace UGH.Application.Reviews;

public class GetAllReviewsQueryHandler
    : IRequestHandler<GetAllReviewsQuery, Result<PaginatedList<Review>>>
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

    public async Task<Result<PaginatedList<Review>>> Handle(
        GetAllReviewsQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedReviews = await _reviewRepository.GetAllReviewsAsync(
                request.PageNumber,
                request.PageSize
            );

            var reviewDtos = paginatedReviews
                .Items.Select(r => new Review
                {
                    Id = r.Id,
                    OfferId = r.OfferId,
                    ReviewerId = r.ReviewerId,
                    ReviewedId = r.ReviewedId,
                    RatingValue = r.RatingValue,
                    ReviewComment = r.ReviewComment,
                    CreatedAt = r.CreatedAt,
                })
                .ToList();

            return Result.Success(
                PaginatedList<Review>.Create(
                    reviewDtos,
                    paginatedReviews.TotalCount,
                    request.PageNumber,
                    request.PageSize
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<PaginatedList<Review>>(
                Errors.General.InvalidOperation("Something went wrong while fetching the reviews.")
            );
        }
    }
}
