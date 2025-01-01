using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
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

    public async Task<PaginatedList<ReviewDto>> GetAllReviewsByUserIdAsync(
        Guid userId,
        int pageNumber,
        int pageSize
    )
    {
        try
        {
            IQueryable<Review> query = _context
                .reviews.Include(r => r.Offer)
                .Where(r => r.ReviewedId == userId);

            int totalCount = await query.CountAsync();

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<ReviewDto>()
                .ToListAsync();

            return PaginatedList<ReviewDto>.Create(reviews, totalCount, pageNumber, pageSize);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PaginatedList<Review>> GetAllReviewsAsync(int pageNumber, int pageSize)
    {
        try
        {
            IQueryable<Review> query = _context
                .reviews.Include(r => r.Reviewed)
                .Include(r => r.Reviewer);

            int totalCount = await query.CountAsync();

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PaginatedList<Review>.Create(reviews, totalCount, pageNumber, pageSize);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Offer> GetOfferByIdAsync(int offerId)
    {
        return await _context.offers.FirstOrDefaultAsync(o => o.Id == offerId);
    }

    public async Task<OfferApplication> GetApprovedApplicationAsync(int offerId, Guid userId)
    {
        return await _context.offerapplication.FirstOrDefaultAsync(app =>
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
        return await _context.reviews.FirstOrDefaultAsync(r =>
            r.OfferId == offerId && r.ReviewerId == reviewerId
        );
    }

    public async Task<PaginatedList<ReviewDto>> GetReviewsByOfferIdAsync(
        int offerId,
        int pageNumber,
        int pageSize
    )
    {
        try
        {
            IQueryable<Review> query = _context
                .reviews.Include(r => r.Offer)
                .Where(r => r.OfferId == offerId);

            int totalCount = await query.CountAsync();

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<ReviewDto>()
                .ToListAsync();

            return PaginatedList<ReviewDto>.Create(reviews, totalCount, pageNumber, pageSize);
        }
        catch (Exception)
        {
            throw;
        }
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

    public async Task<Review> GetReviewByOfferAndUserForHostAsync(int offerId, Guid reviewedId)
    {
        return await _context.reviews.FirstOrDefaultAsync(r =>
            r.OfferId == offerId && r.ReviewedId == reviewedId
        );
    }
}
