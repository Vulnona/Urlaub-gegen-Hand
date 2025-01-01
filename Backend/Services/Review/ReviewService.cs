using Microsoft.Extensions.Logging;
using UGH.Contracts.Review;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Infrastructure.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _repository;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(IReviewRepository repository, ILogger<ReviewService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> AddReviewAsync(
        CreateReviewRequest reviewDto,
        string email,
        Guid? specifiedReviewedId = null
    )
    {
        var reviewer = await _repository.GetUserByEmailAsync(email);
        if (reviewer == null)
        {
            return "User with the provided email does not exist.";
        }

        var offer = await _repository.GetOfferByIdAsync(reviewDto.OfferId);
        if (offer == null)
        {
            return $"Offer not found with the provided OfferId: {reviewDto.OfferId}";
        }

        bool isReviewerHost = reviewer.User_Id == offer.HostId;
        Guid reviewedId;

        if (isReviewerHost)
        {
            if (!specifiedReviewedId.HasValue)
            {
                return "ReviewedId must be specified when the reviewer is a Host.";
            }

            var approvedApplication = await _repository.GetApprovedApplicationAsync(
                reviewDto.OfferId,
                specifiedReviewedId.Value
            );

            if (approvedApplication == null)
            {
                return "No user found for this offer.";
            }

            reviewedId = specifiedReviewedId.Value;
        }
        else
        {
            var approvedApplication = await _repository.GetApprovedApplicationAsync(
                reviewDto.OfferId,
                reviewer.User_Id
            );
            if (approvedApplication == null)
            {
                return "User does not have an approved application for this offer.";
            }
            reviewedId = offer.HostId;
        }

        var existingReview = await _repository.GetReviewByOfferAndUserAsync(
            reviewDto.OfferId,
            reviewer.User_Id
        );
        if (existingReview != null)
        {
            return "A review by this reviewer for this offer already exists.";
        }

        var review = new Review
        {
            OfferId = reviewDto.OfferId,
            RatingValue = reviewDto.RatingValue,
            ReviewComment = reviewDto.ReviewComment,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReviewerId = reviewer.User_Id,
            ReviewedId = reviewedId,
        };

        await _repository.AddReviewAsync(review);
        await _repository.SaveChangesAsync();

        return "Review added successfully.";
    }

    public async Task<PaginatedList<Review>> GetAllReviewsAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllReviewsAsync(pageNumber, pageSize);
    }

    public async Task<string> UpdateReviewAsync(
        int reviewId,
        CreateReviewRequest reviewDto,
        string email
    )
    {
        var user = await _repository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return "User with the provided email does not exist.";
        }

        var review = await _repository.GetReviewByIdAsync(reviewId);
        if (review == null)
        {
            return $"Review not found with the provided ReviewId: {reviewId}";
        }

        if (review.ReviewerId != user.User_Id)
        {
            return "You are not authorized to update this review.";
        }

        var daysSinceCreation = (DateTime.UtcNow - review.CreatedAt).TotalDays;
        if (daysSinceCreation > 14)
        {
            return "Review can only be updated within 14 days of creation.";
        }

        review.RatingValue = reviewDto.RatingValue;
        review.ReviewComment = reviewDto.ReviewComment;
        review.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return "Review updated successfully.";
    }

    public async Task<PaginatedList<ReviewDto>> GetAllReviewsByUserIdAsync(
        Guid userId,
        int pageNumber,
        int pageSize
    )
    {
        return await _repository.GetAllReviewsByUserIdAsync(userId, pageNumber, pageSize);
    }

    public async Task<string> DeleteReviewAsync(int reviewId, string email)
    {
        var user = await _repository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return "User with the provided email does not exist.";
        }

        var review = await _repository.GetReviewByIdAsync(reviewId);
        if (review == null)
        {
            return $"Review not found with the provided ReviewId: {reviewId}";
        }

        if (review.ReviewerId != user.User_Id)
        {
            return "You are not authorized to delete this review.";
        }

        var daysSinceCreation = (DateTime.UtcNow - review.CreatedAt).TotalDays;
        if (daysSinceCreation > 14)
        {
            return "Review can only be deleted within 14 days of creation.";
        }

        _repository.DeleteReviewAsync(review);
        await _repository.SaveChangesAsync();

        return "Review deleted successfully.";
    }
}
