using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Reviews;

public class GetAllReviewsByOfferIdQueryHandler
    : IRequestHandler<GetAllReviewsByOfferIdQuery, Result<List<ReviewDto>>>
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

    public async Task<Result<List<ReviewDto>>> Handle(
        GetAllReviewsByOfferIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var reviews = await _reviewRepository.GetReviewsByOfferIdAsync(request.OfferId);

            if (reviews == null || !reviews.Any())
            {
                return Result.Failure<List<ReviewDto>>(
                    Errors.General.InvalidOperation("Review not found!")
                );
            }

            return Result.Success<List<ReviewDto>>(reviews.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<List<ReviewDto>>(
                Errors.General.InvalidOperation(
                    "Internal server error occurred while fetching user reviews."
                )
            );
        }
    }
}
