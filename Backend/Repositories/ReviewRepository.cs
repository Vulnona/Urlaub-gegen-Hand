using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;
using UGHApi.DATA;

namespace UGHApi.Repositories;

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
            
            // Handle deleted users for reviews
            await HandleDeletedUsersInReviews(reviews);
            
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

            // Handle deleted users for reviews
            await HandleDeletedUsersInReviews(reviews);

            return PaginatedList<ReviewDto>.Create(reviews, totalCount, pageNumber, pageSize);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Get all reviews between two specific users (direct user-to-user reviews)
    /// </summary>
    public async Task<PaginatedList<ReviewDto>> GetReviewsBetweenUsersAsync(Guid user1Id, Guid user2Id, int pageNumber, int pageSize)
    {
        try {
            IQueryable<Review> query = _context
                .reviews.Include(r => r.Offer)
                .Where(r => (r.ReviewerId == user1Id && r.ReviewedId == user2Id) ||
                           (r.ReviewerId == user2Id && r.ReviewedId == user1Id));

            int totalCount = await query.CountAsync();

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<ReviewDto>()
                .ToListAsync();

            // Handle deleted users for reviews
            await HandleDeletedUsersInReviews(reviews);

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
    public async Task<String> AddReview(int? OfferId, int RatingValue, string? ReviewComment, Guid UserId, Guid? ReviewedUserId = null)
    {   
        var reviewer = await _context.users.FindAsync(UserId);
        if (reviewer == null)
            return "Reviewer user not found.";

        Guid reviewedId;
        
        // If OfferId is provided, validate the offer and determine reviewed user
        if (OfferId.HasValue)
        {
            var offer = await GetOfferByIdAsync(OfferId.Value);
            if (offer == null)
                return "Offer not found";
            
            bool isReviewerHost = reviewer.User_Id == offer.UserId;
            
            if (isReviewerHost) {
                if (!ReviewedUserId.HasValue)
                    return "Reviewed User ID is required when reviewer is the host";
                reviewedId = ReviewedUserId.Value;
            }
            else
            {
                reviewedId = offer.UserId;
            }
        }
        else
        {
            // Direct user-to-user review without offer
            if (!ReviewedUserId.HasValue)
                return "Reviewed User ID is required for direct user-to-user reviews";
            reviewedId = ReviewedUserId.Value;
        }

        // Check if review already exists
        Review? existingReview;
        if (OfferId.HasValue)
        {
            existingReview = await _context.reviews.FirstOrDefaultAsync(r => 
                r.OfferId == OfferId.Value && 
                r.ReviewerId == UserId && 
                r.ReviewedId == reviewedId);
        }
        else
        {
            existingReview = await _context.reviews.FirstOrDefaultAsync(r => 
                r.OfferId == null && 
                r.ReviewerId == UserId && 
                r.ReviewedId == reviewedId);
        }

        if (existingReview != null)
            return "Review already exists.";

        // If offer is involved, validate application approval
        if (OfferId.HasValue)
        {
            var approvedApplication = await _context.offerapplication.FirstOrDefaultAsync(app => 
                app.OfferId == OfferId.Value && 
                app.UserId == (reviewer.User_Id == reviewedId ? UserId : reviewedId) && 
                app.Status == OfferApplicationStatus.Approved);
            
            if (approvedApplication == null)
                return "Application not approved.";
        }

        var review = new Review
        {
            OfferId = OfferId,  // Can be null for direct user-to-user reviews
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

    public async Task<PaginatedList<ReviewDto>> GetAllReviewsAdminAsync(int pageNumber, int pageSize)
    {
        IQueryable<Review> query = _context.reviews
            .Include(r => r.Offer)
            .Include(r => r.Reviewer)
            .Include(r => r.Reviewed);

        int totalCount = await query.CountAsync();
        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectToType<ReviewDto>()
            .ToListAsync();

        return PaginatedList<ReviewDto>.Create(reviews, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Handles deleted users in reviews by checking DeletedUserBackups table
    /// </summary>
    private async Task HandleDeletedUsersInReviews(List<ReviewDto> reviews)
    {
        var reviewerIds = reviews.Select(r => r.Reviewer.User_Id).Distinct().ToList();
        var reviewedIds = reviews.Select(r => r.Reviewed.User_Id).Distinct().ToList();
        var allUserIds = reviewerIds.Concat(reviewedIds).Distinct().ToList();

        // Get deleted user backups
        var deletedUserBackups = await _context.DeletedUserBackups
            .Where(b => allUserIds.Contains(Guid.Parse(b.UserId)))
            .ToListAsync();

        foreach (var review in reviews)
        {
            // Check if reviewer is deleted
            var deletedReviewer = deletedUserBackups.FirstOrDefault(b => b.UserId == review.Reviewer.User_Id.ToString());
            if (deletedReviewer != null)
            {
                review.Reviewer.IsDeleted = true;
                review.Reviewer.DeletedUserName = "Gelöschter Nutzer";
                review.Reviewer.FirstName = deletedReviewer.FirstName ?? "Gelöschter";
                review.Reviewer.LastName = deletedReviewer.LastName ?? "Nutzer";
            }

            // Check if reviewed user is deleted
            var deletedReviewed = deletedUserBackups.FirstOrDefault(b => b.UserId == review.Reviewed.User_Id.ToString());
            if (deletedReviewed != null)
            {
                review.Reviewed.IsDeleted = true;
                review.Reviewed.DeletedUserName = "Gelöschter Nutzer";
                review.Reviewed.FirstName = deletedReviewed.FirstName ?? "Gelöschter";
                review.Reviewed.LastName = deletedReviewed.LastName ?? "Nutzer";
            }

            // Check if offer is deleted (offer owner was deleted) - only if offer exists
            if (review.Offer != null)
            {
                var deletedOfferOwner = deletedUserBackups.FirstOrDefault(b => b.UserId == review.Offer.User.User_Id.ToString());
                if (deletedOfferOwner != null)
                {
                    review.Offer.IsDeleted = true;
                    review.Offer.DeletedOfferTitle = "Gelöschtes Angebot";
                    review.Offer.Title = review.Offer.Title ?? "Gelöschtes Angebot";
                    review.Offer.User.IsDeleted = true;
                    review.Offer.User.DeletedUserName = "Gelöschter Nutzer";
                    review.Offer.User.FirstName = deletedOfferOwner.FirstName ?? "Gelöschter";
                    review.Offer.User.LastName = deletedOfferOwner.LastName ?? "Nutzer";
                }
            }
        }
    }
}
