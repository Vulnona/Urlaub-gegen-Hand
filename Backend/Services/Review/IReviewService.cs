using UGH.Contracts.Review;
using UGH.Domain.Entities;
using UGHApi.Shared;
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
    Task<PaginatedList<ReviewDto>> GetAllReviewsByUserIdAsync(
        Guid userId,
        int pageNumber,
        int pageSize
    );
    Task<PaginatedList<Review>> GetAllReviewsAsync(int pageNumber, int pageSize);
}
