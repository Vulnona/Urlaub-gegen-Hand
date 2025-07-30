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
            Console.WriteLine($"[DEBUG] GetAllReviewsByUserIdAsync called for userId: {userId}");
            
            // Query without includes first to get all reviews
            var allReviews = await _context.reviews
                .Where(r => r.ReviewedId == userId)
                .ToListAsync();
            
            Console.WriteLine($"[DEBUG] Found {allReviews.Count} reviews in database");
            
            // Temporarily show all reviews for debugging
            var visibleReviews = allReviews;
            
            // Apply pagination to visible reviews
            int totalCount = visibleReviews.Count;
            var paginatedReviews = visibleReviews
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            Console.WriteLine($"[DEBUG] Converting {paginatedReviews.Count} reviews to DTOs");
            
            // Convert to DTOs using Mapster (now properly configured)
            var reviewDtos = paginatedReviews.Select(r => r.Adapt<ReviewDto>()).ToList();
            
            Console.WriteLine($"[DEBUG] Successfully converted to DTOs, now handling deleted users");
            
            // Handle deleted users in reviews
            await HandleDeletedUsersInReviews(reviewDtos);
            
            Console.WriteLine($"[DEBUG] Returning {reviewDtos.Count} reviews");
            
            return PaginatedList<ReviewDto>.Create(reviewDtos, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Exception in GetAllReviewsByUserIdAsync: {ex.Message}");
            Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    private async Task HandleDeletedUsersInReviews(List<ReviewDto> reviews)
    {
        try
        {
            foreach (var review in reviews)
            {
                // If reviewer is null, it means the user was deleted
                if (review.Reviewer == null)
                {
                    review.Reviewer = new UGHApi.ViewModels.UserComponent.UserC
                    {
                        User_Id = Guid.Empty, // We don't have the original ID
                        FirstName = "Gelöschter",
                        LastName = "Nutzer",
                        IsDeleted = true
                    };
                }
                else
                {
                    // Check if reviewer still exists
                    try
                    {
                        var reviewerExists = await _context.users.AnyAsync(u => u.User_Id == review.Reviewer.User_Id);
                        if (!reviewerExists)
                        {
                            // Mark as deleted user
                            review.Reviewer.FirstName = "Gelöschter";
                            review.Reviewer.LastName = "Nutzer";
                            review.Reviewer.IsDeleted = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Error checking reviewer {review.Reviewer.User_Id}: {ex.Message}");
                        // Mark as deleted user as fallback
                        review.Reviewer.FirstName = "Gelöschter";
                        review.Reviewer.LastName = "Nutzer";
                        review.Reviewer.IsDeleted = true;
                    }
                }

                // If reviewed is null, it means the user was deleted
                if (review.Reviewed == null)
                {
                    review.Reviewed = new UGHApi.ViewModels.UserComponent.UserC
                    {
                        User_Id = Guid.Empty, // We don't have the original ID
                        FirstName = "Gelöschter",
                        LastName = "Nutzer",
                        IsDeleted = true
                    };
                }
                else
                {
                    // Check if reviewed user still exists
                    try
                    {
                        var reviewedExists = await _context.users.AnyAsync(u => u.User_Id == review.Reviewed.User_Id);
                        if (!reviewedExists)
                        {
                            // Mark as deleted user
                            review.Reviewed.FirstName = "Gelöschter";
                            review.Reviewed.LastName = "Nutzer";
                            review.Reviewed.IsDeleted = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Error checking reviewed {review.Reviewed.User_Id}: {ex.Message}");
                        // Mark as deleted user as fallback
                        review.Reviewed.FirstName = "Gelöschter";
                        review.Reviewed.LastName = "Nutzer";
                        review.Reviewed.IsDeleted = true;
                    }
                }

                // Check if offer is deleted
                if (review.Offer == null && review.OfferId.HasValue)
                {
                    review.Offer = new UGH.Domain.Entities.Offer
                    {
                        Id = review.OfferId.Value,
                        Title = "Gelöschtes Angebot",
                        Description = "Dieses Angebot wurde gelöscht",
                        UserId = Guid.Empty,
                        IsDeleted = true
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Exception in HandleDeletedUsersInReviews: {ex.Message}");
            Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
            // Don't throw, just log the error
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
                .Where(r => r.OfferId == offerId && r.Offer.UserId == r.ReviewedId);

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
        
        bool isReviewerHost = reviewer.User_Id == offer.UserId;
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
            reviewedId = offer.UserId;
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
        var review = new Review
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
}
