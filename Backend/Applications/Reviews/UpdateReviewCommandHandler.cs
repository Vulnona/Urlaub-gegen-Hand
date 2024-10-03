using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UGH.Application.Reviews;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Result>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository; // Optional if user verification is required
    private readonly ILogger<UpdateReviewCommandHandler> _logger;

    public UpdateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUserRepository userRepository,
        ILogger<UpdateReviewCommandHandler> logger
    )
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateReviewCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure(Errors.General.NotFound("UserNotFound", user));
            }

            var review = await _reviewRepository.GetReviewByIdAsync(
                request.updateReviewRequest.ReviewId
            );
            if (review == null)
            {
                return Result.Failure(Errors.General.NotFound("ReviewNotFound", review));
            }

            review.RatingValue = request.updateReviewRequest.RatingValue;
            review.ReviewComment = request.updateReviewRequest.ReviewComment;

            await _reviewRepository.UpdateReviewAsync(review);
            _logger.LogInformation("Review updated successfully.");

            return Result.Success("Review updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation("Something went wrong while updating review.")
            );
        }
    }
}
