using MediatR;
using Microsoft.Extensions.Logging;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository; // Optional if user verification is required
    private readonly ILogger<DeleteReviewCommandHandler> _logger;

    public DeleteReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUserRepository userRepository,
        ILogger<DeleteReviewCommandHandler> logger
    )
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteReviewCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // Validate user existence and authorization
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure(
                    Errors.General.InvalidOperation(
                        "Error: User with the provided email does not exist."
                    )
                );
            }

            // Fetch the review to delete
            var review = await _reviewRepository.GetReviewByIdAsync(request.ReviewId);
            if (review == null)
            {
                return Result.Failure(Errors.General.InvalidOperation("Error: Review not found."));
            }

            // Check if the user is authorized to delete the review (if applicable)
            if (review.ReviewerId != user.User_Id)
            {
                return Result.Failure(
                    Errors.General.InvalidOperation("Error: Unauthorized to delete this review.")
                );
            }

            // Delete the review
            _reviewRepository.DeleteReviewAsync(review);
            _logger.LogInformation("Review deleted successfully.");

            return Result.Success("Review deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation("Something went wrong while deleting review")
            );
        }
    }
}
