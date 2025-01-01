using UGH.Domain.Entities;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Domain.Interfaces;

public interface IReviewRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<PaginatedList<ReviewDto>> GetReviewsByOfferIdAsync(
        int offerId,
        int pageNumber,
        int pageSize
    );
    Task<PaginatedList<Review>> GetAllReviewsAsync(int pageNumber, int pageSize);
    Task<PaginatedList<ReviewDto>> GetAllReviewsByUserIdAsync(
        Guid userId,
        int pageNumber,
        int pageSize
    );
    Task<Offer> GetOfferByIdAsync(int offerId);
    Task<OfferApplication> GetApprovedApplicationAsync(int offerId, Guid userId);
    Task<Review> GetReviewByIdAsync(int reviewId);
    Task<Review> GetReviewByOfferAndUserAsync(int offerId, Guid reviewerId);
    Task<Review> GetReviewByOfferAndUserForHostAsync(int offerId, Guid reviewedId);
    Task AddReviewAsync(Review review);
    Task UpdateReviewAsync(Review review);
    void DeleteReviewAsync(Review review);
    Task SaveChangesAsync();
}
