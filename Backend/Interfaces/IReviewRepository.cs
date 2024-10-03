using UGH.Domain.Entities;
using UGHApi.ViewModels;

namespace UGH.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<List<ReviewDto>> GetReviewsByOfferIdAsync(int offerId);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewDto>> GetAllReviewsByUserIdAsync(Guid userId);
        Task<Offer> GetOfferByIdAsync(int offerId);
        Task<OfferApplication> GetApprovedApplicationAsync(int offerId, Guid userId);
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<Review> GetReviewByOfferAndUserAsync(int offerId, Guid reviewerId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        void DeleteReviewAsync(Review review);
        Task SaveChangesAsync();
    }
}
