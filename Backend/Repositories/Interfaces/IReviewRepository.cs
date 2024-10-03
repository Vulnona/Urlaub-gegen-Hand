using UGH.Domain.Entities;

namespace UGHApi.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Review>> GetAllReviewsByUserIdAsync(Guid userId);
        Task<Offer> GetOfferByIdAsync(int offerId);
        Task<OfferApplication> GetApprovedApplicationAsync(int offerId, Guid userId);
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<Review> GetReviewByOfferAndUserAsync(int offerId, Guid reviewerId);
        Task AddReviewAsync(Review review);
        void DeleteReviewAsync(Review review);
        Task SaveChangesAsync();
    }
}
