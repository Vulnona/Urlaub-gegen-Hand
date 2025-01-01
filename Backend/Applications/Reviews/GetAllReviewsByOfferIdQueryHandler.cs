using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Reviews;

public class GetAllReviewsByOfferIdQueryHandler
    : IRequestHandler<GetAllReviewsByOfferIdQuery, Result<PaginatedList<ReviewDto>>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<GetAllReviewsByOfferIdQueryHandler> _logger;

    public GetAllReviewsByOfferIdQueryHandler(
        IReviewRepository reviewRepository,
        ILogger<GetAllReviewsByOfferIdQueryHandler> logger
    )
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<ReviewDto>>> Handle(
        GetAllReviewsByOfferIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedReviews = await _reviewRepository.GetReviewsByOfferIdAsync(
                request.OfferId,
                request.PageNumber,
                request.PageSize
            );

            if (paginatedReviews.Items == null || !paginatedReviews.Items.Any())
            {
                return Result.Failure<PaginatedList<ReviewDto>>(
                    Errors.General.InvalidOperation("No reviews found for this offer.")
                );
            }

            return Result.Success(paginatedReviews);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<PaginatedList<ReviewDto>>(
                Errors.General.InvalidOperation(
                    "Internal server error occurred while fetching offer reviews."
                )
            );
        }
    }
}
