using UGH.Domain.Entities;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Domain.Interfaces;

public interface IReviewRepository
{
    Task<PaginatedList<ReviewDto>> GetReviewsByOfferIdAsync(
        int offerId,
        int pageNumber,
        int pageSize
    );
    Task<PaginatedList<ReviewDto>> GetAllReviewsByUserIdAsync(
        Guid userId,
        int pageNumber,
        int pageSize
    );
    Task<Offer> GetOfferByIdAsync(int offerId);
    Task<Review> GetReviewByIdAsync(int reviewId);
    Task UpdateReviewAsync(Review review);
    void DeleteReviewAsync(Review review);
    #nullable enable
    Task<String> AddReview(int OfferId, int RatingValue, string? ReviewComment, Guid UserId, Guid? ReviewedUserId);
    #nullable disable
}
