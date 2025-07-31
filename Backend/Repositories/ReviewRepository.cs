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
            
            // Query reviews where user is the reviewed person (recipient)
            // Don't use Include for Reviewer/Reviewed as they might be deleted
            var allReviews = await _context.reviews
                .Include(r => r.Offer)
                .Where(r => r.ReviewedId == userId)
                .ToListAsync();
            
            Console.WriteLine($"[DEBUG] Found {allReviews.Count} reviews in database");
            
            // Apply visibility logic: only show reviews that are visible
            var visibleReviews = allReviews.Where(r => IsReviewVisible(r)).ToList();
            
            Console.WriteLine($"[DEBUG] {visibleReviews.Count} reviews are visible");
            
            // Apply pagination to visible reviews
            int totalCount = visibleReviews.Count;
            var paginatedReviews = visibleReviews
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            Console.WriteLine($"[DEBUG] Converting {paginatedReviews.Count} reviews to DTOs");
            
            // Convert to DTOs using Mapster
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

    private bool IsReviewVisible(Review review)
    {
        // Check if review is explicitly marked as visible
        if (review.IsVisible)
            return true;
        
        // Check if 14 days have passed since creation
        if (review.VisibilityDate.HasValue && DateTime.UtcNow >= review.VisibilityDate.Value)
            return true;
        
        // Check if both parties have reviewed each other (mutual review)
        if (review.OfferId.HasValue)
        {
            // This would need to be implemented based on your business logic
            // For now, we'll use the 14-day rule as default
            return false;
        }
        
        return false;
    }

    private async Task HandleDeletedUsersInReviews(List<ReviewDto> reviews)
    {
        try
        {
            foreach (var review in reviews)
            {
                // Handle Reviewer - check if user still exists in database
                bool reviewerExists = await UserExists(review.ReviewerId);
                
                if (!reviewerExists)
                {
                    // User was deleted, show "Gelöschter Nutzer"
                    review.Reviewer = new UGHApi.ViewModels.UserComponent.UserC
                    {
                        User_Id = review.ReviewerId,
                        FirstName = "Gelöschter",
                        LastName = "Nutzer",
                        IsDeleted = true
                    };
                }
                else
                {
                    // User still exists, use stored information or fetch from database
                    if (!string.IsNullOrEmpty(review.ReviewerFirstName) && !string.IsNullOrEmpty(review.ReviewerLastName))
                    {
                        // Use stored reviewer information
                        review.Reviewer = new UGHApi.ViewModels.UserComponent.UserC
                        {
                            User_Id = review.ReviewerId,
                            FirstName = review.ReviewerFirstName,
                            LastName = review.ReviewerLastName,
                            IsDeleted = false
                        };
                    }
                    else
                    {
                        // Fallback: mark as deleted if no stored info
                        review.Reviewer = new UGHApi.ViewModels.UserComponent.UserC
                        {
                            User_Id = review.ReviewerId,
                            FirstName = "Gelöschter",
                            LastName = "Nutzer",
                            IsDeleted = true
                        };
                    }
                }

                // Handle Reviewed user - should always be the current user
                review.Reviewed = new UGHApi.ViewModels.UserComponent.UserC
                {
                    User_Id = review.ReviewedId,
                    FirstName = "Current", // This will be replaced by the actual user data
                    LastName = "User",
                    IsDeleted = false
                };

                // Check if offer is deleted
                if (review.Offer == null && review.OfferId.HasValue)
                {
                    review.Offer = new UGH.Domain.Entities.Offer
                    {
                        Id = review.OfferId.Value,
                        Title = "Gelöschtes Angebot",
                        Description = "Dieses Angebot wurde gelöscht",
                        UserId = Guid.Empty
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

    private async Task<bool> UserExists(Guid userId)
    {
        try
        {
            return await _context.users.AnyAsync(u => u.User_Id == userId);
        }
        catch
        {
            return false;
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
        try
        {
            Console.WriteLine($"[DEBUG] AddReview called with OfferId: {OfferId}, RatingValue: {RatingValue}, UserId: {UserId}, ReviewedUserId: {ReviewedUserId}");
            
            var reviewer = await _context.users.FindAsync(UserId);
            if (reviewer == null)
            {
                Console.WriteLine($"[DEBUG] Reviewer not found for UserId: {UserId}");
                return "Reviewer user not found.";
            }
            Console.WriteLine($"[DEBUG] Found reviewer: {reviewer.FirstName} {reviewer.LastName}");
            
            var offer = await GetOfferByIdAsync(OfferId);
            if (offer == null)
            {
                Console.WriteLine($"[DEBUG] Offer not found for OfferId: {OfferId}");
                return "Offer not found";
            }
            Console.WriteLine($"[DEBUG] Found offer: {offer.Title}");
            
            bool isReviewerHost = reviewer.User_Id == offer.UserId;
            Guid reviewedId;
            Guid guestId;
            
            Console.WriteLine($"[DEBUG] IsReviewerHost: {isReviewerHost}, ReviewerId: {reviewer.User_Id}, OfferUserId: {offer.UserId}");
            
            if (isReviewerHost) {
                if (!ReviewedUserId.HasValue)
                {
                    Console.WriteLine($"[DEBUG] ReviewedUserId is null for host review");
                    return "Reviewed User ID is required for host reviews";
                }
                reviewedId = ReviewedUserId.Value;
                guestId = ReviewedUserId.Value;
            }
            else
            {
                reviewedId = offer.UserId;
                guestId = UserId;
            }
            
            Console.WriteLine($"[DEBUG] ReviewedId: {reviewedId}, GuestId: {guestId}");
            
            var existingReview = await _context.reviews.FirstOrDefaultAsync(r => r.OfferId == OfferId && r.ReviewerId == UserId && r.ReviewedId == reviewedId);
            if (existingReview != null)
            {
                Console.WriteLine($"[DEBUG] Review already exists");
                return "Review already exists.";
            }
            
            Console.WriteLine($"[DEBUG] Checking for approved application: OfferId={OfferId}, UserId={guestId}");
            var approvedApplication = await _context.offerapplication.FirstOrDefaultAsync(app => app.OfferId == OfferId && app.UserId == guestId && app.Status == OfferApplicationStatus.Approved);
            if (approvedApplication == null)
            {
                Console.WriteLine($"[DEBUG] No approved application found");
                return "Application not approved.";
            }
            Console.WriteLine($"[DEBUG] Found approved application: {approvedApplication.Id}");

            // Check if there's already a review from the other party
            var existingOppositeReview = await _context.reviews.FirstOrDefaultAsync(r => 
                r.OfferId == OfferId && 
                r.ReviewerId == reviewedId && 
                r.ReviewedId == reviewer.User_Id);
            
            bool isVisible = false;
            DateTime? visibilityDate = null;
            
            if (existingOppositeReview != null)
            {
                // Both parties have reviewed each other - make both reviews visible immediately
                isVisible = true;
                existingOppositeReview.IsVisible = true;
                _context.reviews.Update(existingOppositeReview);
                Console.WriteLine($"[DEBUG] Both parties have reviewed each other - making reviews visible immediately");
            }
            else
            {
                // Only one party has reviewed - use 14-day rule
                isVisible = false;
                visibilityDate = DateTime.UtcNow.AddDays(14);
                Console.WriteLine($"[DEBUG] Only one party has reviewed - using 14-day rule");
            }

            var review = new Review
            {
                OfferId = OfferId,
                RatingValue = RatingValue,
                ReviewComment = ReviewComment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReviewerId = reviewer.User_Id,
                ReviewedId = reviewedId,
                // Store reviewer information
                ReviewerFirstName = reviewer.FirstName,
                ReviewerLastName = reviewer.LastName,
                ReviewerEmail = reviewer.Email_Address,
                // Set visibility logic
                IsVisible = isVisible,
                VisibilityDate = visibilityDate
            };

            Console.WriteLine($"[DEBUG] Creating review with ReviewerId: {review.ReviewerId}, ReviewedId: {review.ReviewedId}");
            await _context.reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[DEBUG] Review added successfully with ID: {review.Id}");
            return "Review added successfully.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Exception in AddReview: {ex.Message}");
            Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
            return $"Error adding review: {ex.Message}";
        }
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
