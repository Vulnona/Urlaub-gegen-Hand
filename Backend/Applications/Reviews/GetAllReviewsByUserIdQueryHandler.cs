using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

public class GetAllReviewsByUserIdQueryHandler
    : IRequestHandler<GetAllReviewsByUserIdQuery, Result<PaginatedList<ReviewDto>>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<GetAllReviewsByUserIdQueryHandler> _logger;

    public GetAllReviewsByUserIdQueryHandler(
        IReviewRepository reviewRepository,
        ILogger<GetAllReviewsByUserIdQueryHandler> logger
    )
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<ReviewDto>>> Handle(
        GetAllReviewsByUserIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedReviews = await _reviewRepository.GetAllReviewsByUserIdAsync(
                request.UserId,
                request.PageNumber,
                request.PageSize
            );

            if (paginatedReviews.Items == null || !paginatedReviews.Items.Any())
            {
                return Result.Failure<PaginatedList<ReviewDto>>(
                    Errors.General.InvalidOperation("No reviews found for this user.")
                );
            }

            return Result.Success(paginatedReviews);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<PaginatedList<ReviewDto>>(
                Errors.General.InvalidOperation("Error occurred while fetching user reviews.")
            );
        }
    }
}
