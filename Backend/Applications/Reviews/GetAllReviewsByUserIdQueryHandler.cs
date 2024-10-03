using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using UGHApi.ViewModels;

public class GetAllReviewsByUserIdQueryHandler
    : IRequestHandler<GetAllReviewsByUserIdQuery, Result<List<ReviewDto>>>
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

    public async Task<Result<List<ReviewDto>>> Handle(
        GetAllReviewsByUserIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var reviews = await _reviewRepository.GetAllReviewsByUserIdAsync(request.UserId);

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
