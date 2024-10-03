using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Reviews;

public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;

    public AddReviewCommandHandler(
        IUserRepository userRepository,
        IReviewRepository reviewRepository
    )
    {
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var specifiedReviewedId = request.ReviewedUserId;

        var reviewer = await _userRepository.GetUserByIdAsync(request.UserId);
        if (reviewer == null)
        {
            return Result.Failure(Errors.General.NotFound("User", request.UserId));
        }

        var offer = await _reviewRepository.GetOfferByIdAsync(request.OfferId);
        if (offer == null)
        {
            return Result.Failure(Errors.General.NotFound("Offer", request.OfferId));
        }

        bool isReviewerHost = reviewer.User_Id == offer.HostId;
        Guid reviewedId;

        if (isReviewerHost)
        {
            if (!specifiedReviewedId.HasValue)
            {
                return Result.Failure(
                    Errors.General.NotFound("Reviewed User", specifiedReviewedId)
                );
            }

            var approvedApplication = await _reviewRepository.GetApprovedApplicationAsync(
                request.OfferId,
                specifiedReviewedId.Value
            );

            if (approvedApplication == null)
            {
                return Result.Failure(
                    Errors.General.NotFound("Approved Application", specifiedReviewedId.Value)
                );
            }

            reviewedId = specifiedReviewedId.Value;
        }
        else
        {
            var approvedApplication = await _reviewRepository.GetApprovedApplicationAsync(
                request.OfferId,
                reviewer.User_Id
            );
            if (approvedApplication == null)
            {
                return Result.Failure(
                    Errors.General.NotAuthorized("submit a review for this offer")
                );
            }

            reviewedId = offer.HostId;
        }

        var existingReview = await _reviewRepository.GetReviewByOfferAndUserAsync(
            request.OfferId,
            reviewer.User_Id
        );
        if (existingReview != null)
        {
            return Result.Failure(Errors.General.AlreadyExists("Review", request.OfferId));
        }

        var review = new Domain.Entities.Review
        {
            OfferId = request.OfferId,
            RatingValue = request.RatingValue,
            ReviewComment = request.ReviewComment,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReviewerId = reviewer.User_Id,
            ReviewedId = reviewedId
        };

        await _reviewRepository.AddReviewAsync(review);
        await _reviewRepository.SaveChangesAsync();

        return Result.Success("Review added successfully.");
    }
}
