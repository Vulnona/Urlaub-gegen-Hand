using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Infrastructure.Repositories;

public class ReviewRepository
{
    private readonly Ugh_Context _context;

    public ReviewRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ReviewDto>> GetAllReviewsByUserIdAsync(Guid userId, int pageNumber, int pageSize)
    {
        try {
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

    public async Task<Offer> GetOfferByIdAsync(int offerId)
    {
        return await _context.offers.FirstOrDefaultAsync(o => o.Id == offerId);
    }

    public async Task<Review> GetReviewByIdAsync(int reviewId)
    {
        return await _context.reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
    }

    public async Task<PaginatedList<ReviewDto>> GetReviewsByOfferIdAsync(int offerId, int pageNumber, int pageSize) {
        try {
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

    public async Task UpdateReviewAsync(Review review)
    {
        _context.reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public void DeleteReviewAsync(Review review)
    {
        _context.reviews.Remove(review);
    }

#nullable enable
    public async Task<String> AddReview(int OfferId, int RatingValue, string? ReviewComment, Guid UserId, Guid? ReviewedUserId = null)
    {   
        var reviewer = await _context.users.FindAsync(UserId);
        if (reviewer == null)
            return "Reviewed user not found.";
        var offer = await GetOfferByIdAsync(OfferId);
        if (offer == null)
            return "Offer not found";
        
        bool isReviewerHost = reviewer.User_Id == offer.HostId;
        Guid reviewedId;
        Guid guestId;
        
        Review? existingReview = null;
        if (isReviewerHost) {
            if (!ReviewedUserId.HasValue)
                return "Reviewed User {ReviewedUserId} not found";
            reviewedId = ReviewedUserId.Value;
            guestId = ReviewedUserId.Value;
        }
        else
        {
            reviewedId = offer.HostId;
            guestId = UserId;
        }
        existingReview =  await _context.reviews.FirstOrDefaultAsync(r => r.OfferId == OfferId && r.ReviewerId == UserId && r.ReviewedId == reviewedId);
        if (existingReview != null)
            return "Review already exists.";        
        
        var approvedApplication = await _context.offerapplication.FirstOrDefaultAsync(app => app.OfferId == OfferId && app.UserId == guestId  && app.Status == OfferApplicationStatus.Approved);
        if (approvedApplication == null)
            return "Application not approved.";

        
        if (existingReview != null)
            return "Review already exists";
        var review = new Domain.Entities.Review
        {
            OfferId = OfferId,
            RatingValue = RatingValue,
            ReviewComment = ReviewComment,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReviewerId = reviewer.User_Id,
            ReviewedId = reviewedId,
        };

        await _context.reviews.AddAsync(review);
        await _context.SaveChangesAsync();


        return "Review added successfully.";
    }
#nullable disable    
}
