using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.ViewModels;

namespace UGH.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly Ugh_Context _context;

    public ReviewRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.Email_Address == email);
    }

    public async Task<IEnumerable<ReviewDto>> GetAllReviewsByUserIdAsync(Guid userId)
    {
        return await _context.reviews
            .Include(r => r.Offer)
            .Where(r => r.ReviewedId == userId)
            .ProjectToType<ReviewDto>()
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _context.reviews
            .Include(r => r.Reviewed)
            .Include(r => r.Reviewer)
            .ToListAsync();
    }

    public async Task<Offer> GetOfferByIdAsync(int offerId)
    {
        return await _context.offers.FirstOrDefaultAsync(o => o.Id == offerId);
    }

    public async Task<OfferApplication> GetApprovedApplicationAsync(int offerId, Guid userId)
    {
        return await _context.offerapplication.FirstOrDefaultAsync(
            app =>
                app.OfferId == offerId
                && app.UserId == userId
                && app.Status == OfferApplicationStatus.Approved
        );
    }

    public async Task<Review> GetReviewByIdAsync(int reviewId)
    {
        return await _context.reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
    }

    public async Task<Review> GetReviewByOfferAndUserAsync(int offerId, Guid reviewerId)
    {
        return await _context.reviews.FirstOrDefaultAsync(
            r => r.OfferId == offerId && r.ReviewerId == reviewerId
        );
    }

    public async Task<List<ReviewDto>> GetReviewsByOfferIdAsync(int offerId)
    {
        return await _context.reviews
            .Include(r => r.Offer)
            .Where(r => r.OfferId == offerId)
            .ProjectToType<ReviewDto>()
            .ToListAsync();
    }

    public async Task AddReviewAsync(Review review)
    {
        await _context.reviews.AddAsync(review);
    }

    public async Task UpdateReviewAsync(Review review)
    {
        _context.reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void DeleteReviewAsync(Review review)
    {
        _context.reviews.Remove(review);
    }
}
