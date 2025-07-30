using UGH.Infrastructure.Services;
using UGH.Domain.ViewModels;
using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UGHApi.DATA;

namespace UGH.Application.Profile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDataDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserProfileQueryHandler> _logger;
    private readonly Ugh_Context _context;

    public GetUserProfileQueryHandler(
        ILogger<GetUserProfileQueryHandler> logger,
        IUserRepository userRepository,
        Ugh_Context context
    )
    {
        _logger = logger;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<UserProfileDataDTO> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var userId = request.UserId;

            var user = await _userRepository.GetUserWithRatingByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {userId} not found.");
                return null;
            }
            if (user.Address == null)
            {
                _logger.LogError($"User {userId} has no Address object.");
            }
            if (user.UserMemberships == null)
            {
                _logger.LogError($"User {userId} has no UserMemberships object.");
            }
            if (user.Hobbies == null)
            {
                _logger.LogError($"User {userId} has no Hobbies.");
            }
            if (user.Skills == null)
            {
                _logger.LogError($"User {userId} has no Skills.");
            }
            
            var membershipEndDate = user.UserMemberships?.Where(m => m.IsMembershipActive).OrderBy(m => m.CreatedAt).FirstOrDefault()?.Expiration;
            
            // Calculate user rating from mutual reviews only
            double userRating = 0;
            var allReviews = await _context.reviews.AsQueryable().Where(r => r.ReviewedId == user.User_Id).ToListAsync();
            
            // Filter to only include reviews where both parties have reviewed each other OR after 14 days
            var visibleReviews = new List<Review>();
            
            if (allReviews.Any())
            {
                // Get all reviews for the same offers to check for mutual reviews
                var offerIds = allReviews.Where(r => r.OfferId.HasValue).Select(r => r.OfferId.Value).Distinct().ToList();
                var allReviewsForOffers = await _context.reviews
                    .Where(r => offerIds.Contains(r.OfferId.Value))
                    .ToListAsync();

                foreach (var review in allReviews)
                {
                    if (review.OfferId.HasValue)
                    {
                        // Check if the other party has also reviewed this user for the same offer
                        var otherPartyReview = allReviewsForOffers
                            .FirstOrDefault(r => r.OfferId == review.OfferId && 
                                               r.ReviewerId == review.ReviewedId && 
                                               r.ReviewedId == review.ReviewerId);
                        
                        // Include review if both parties have reviewed each other OR if 14 days have passed
                        bool shouldInclude = otherPartyReview != null || 
                                            (DateTime.UtcNow - review.CreatedAt).TotalDays >= 14;
                        
                        if (shouldInclude)
                        {
                            visibleReviews.Add(review);
                        }
                    }
                }
            }
            
            if (visibleReviews.Count() > 0)
                userRating = Math.Round(visibleReviews.Average(r => r.RatingValue), 1);
            
            // Load skills from skills table if user has skills
            List<object> userSkills = new List<object>();
            if (!string.IsNullOrEmpty(user.Skills))
            {
                _logger.LogWarning($"User Skills string: '{user.Skills}'");
                var skillIds = user.Skills.Split(',')
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .ToList();
                
                _logger.LogWarning($"Parsed skill IDs: [{string.Join(", ", skillIds)}]");
                
                if (skillIds.Any())
                {
                    var skills = await _context.skills
                        .Where(s => skillIds.Contains(s.Skill_ID.ToString()))
                        .ToListAsync();
                    
                    _logger.LogWarning($"Found {skills.Count} skills in database");
                    foreach (var skill in skills)
                    {
                        _logger.LogWarning($"Skill: ID={skill.Skill_ID}, Name='{skill.SkillDescrition}'");
                    }
                    
                    userSkills = skills.Select(s => new { 
                        id = s.Skill_ID, 
                        name = s.SkillDescrition 
                    }).Cast<object>().ToList();
                }
            }
            
            _logger.LogWarning($"Final userSkills count: {userSkills.Count}");
            
            var userProfileDataDTO = new UserProfileDataDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = user.ProfilePicture,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address == null ? null : new AddressDTO {
                    Id = user.Address.Id,
                    DisplayName = user.Address.DisplayName,
                    Latitude = user.Address.Latitude,
                    Longitude = user.Address.Longitude
                },
                FacebookLink = user.Facebook_link,
                Link_RS = user.Link_RS,
                Link_VS = user.Link_VS,
                UserRating = userRating,
                MembershipEndDate = membershipEndDate,
                Hobbies = user?.Hobbies?.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList(),
                Skills = userSkills,
                VerificationState = user.VerificationState.ToString()
            };
            return userProfileDataDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"Error occurred while handling GetUserProfileQuery for UserId: {request.UserId}"
            );
            throw;
        }
    }
}

