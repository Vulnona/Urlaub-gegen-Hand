using UGH.Contracts.Review;
using UGHApi.ViewModels;

namespace UGH.Infrastructure.Services;

public interface IReviewService
{
    Task<string> AddReviewAsync(
        CreateReviewRequest reviewDto,
        string email,
        Guid? specifiedReviewedId = null
    );
    Task<string> UpdateReviewAsync(int reviewId, CreateReviewRequest reviewDto, string email);
    Task<string> DeleteReviewAsync(int reviewId, string email);
    Task<IEnumerable<ReviewDto>> GetAllReviewsByUserIdAsync(Guid userId);
    Task<IEnumerable<Domain.Entities.Review>> GetAllReviewsAsync();
}
