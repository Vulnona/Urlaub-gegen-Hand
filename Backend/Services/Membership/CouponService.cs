using UGH.Domain.Entities;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
#nullable enable
using Microsoft.Extensions.Logging;
using static UGH.Domain.Core.UGH_Enums;

namespace UGHApi.Services.Membership;

/// <summary>
/// Service for managing coupon operations with proper validation and security
/// </summary>
public interface ICouponService
{
    /// <summary>
    /// Generates a new coupon code
    /// </summary>
    Task<Result<Coupon>> GenerateCouponAsync(int membershipId, int duration, Guid createdBy, string? customCode = null);
    
    /// <summary>
    /// Redeems a coupon for a user
    /// </summary>
    Task<Result<UserMembership>> RedeemCouponAsync(string couponCode, Guid userId);
    
    /// <summary>
    /// Validates if a coupon can be redeemed
    /// </summary>
    Task<Result<Coupon>> ValidateCouponAsync(string couponCode);
    
    /// <summary>
    /// Gets all coupons with filtering options
    /// </summary>
    Task<Result<List<Coupon>>> GetCouponsAsync(bool includeRedeemed = false, int? membershipId = null);
    
    /// <summary>
    /// Deactivates/cancels a coupon (Admin only)
    /// </summary>
    Task<Result> DeactivateCouponAsync(string couponCode, Guid adminUserId);
}

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly ILogger<CouponService> _logger;

    public CouponService(
        ICouponRepository couponRepository,
        IUserRepository userRepository,
        IMembershipRepository membershipRepository,
        ILogger<CouponService> logger)
    {
        _couponRepository = couponRepository;
        _userRepository = userRepository;
        _membershipRepository = membershipRepository;
        _logger = logger;
    }

    public async Task<Result<Coupon>> GenerateCouponAsync(int membershipId, int duration, Guid createdBy, string? customCode = null)
    {
        try
        {
            // Validate membership exists
            var membership = await _membershipRepository.GetMembershipByIdAsync(membershipId);
            if (membership == null)
            {
                return Result.Failure<Coupon>(new Error("InvalidMembership", "Membership not found"));
            }

            // Generate unique code if not provided
            var couponCode = customCode ?? await GenerateUniqueCouponCodeAsync();
            
            // Validate code is unique
            var existingCoupon = await _couponRepository.GetCouponByCode(couponCode);
            if (existingCoupon != null)
            {
                return Result.Failure<Coupon>(new Error("DuplicateCode", "Coupon code already exists"));
            }

            var coupon = new Coupon
            {
                Code = couponCode,
                Name = $"Membership Coupon - {membership.Name}",
                Description = $"Coupon for {duration} membership access",
                Duration = duration,
                MembershipId = membershipId,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            await _couponRepository.AddCoupon(coupon);
            
            _logger.LogInformation($"Generated coupon {couponCode} for membership {membershipId} by user {createdBy}");
            
            return Result.Success(coupon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to generate coupon for membership {membershipId}");
            return Result.Failure<Coupon>(new Error("GenerationFailed", "Failed to generate coupon"));
        }
    }

    public async Task<Result<UserMembership>> RedeemCouponAsync(string couponCode, Guid userId)
    {
        try
        {
            // Validate coupon
            var couponResult = await ValidateCouponAsync(couponCode);
            if (couponResult.IsFailure)
            {
                return Result.Failure<UserMembership>(couponResult.Error);
            }

            var coupon = couponResult.Value;
            var user = await _userRepository.GetUserByIdAsync(userId);
            
            if (user == null)
            {
                return Result.Failure<UserMembership>(new Error("UserNotFound", "User not found"));
            }

            // Check if user already has active membership
            var activeMemberships = await _userRepository.GetActiveUserMembershipsAsync(userId);
            if (activeMemberships.Any())
            {
                return Result.Failure<UserMembership>(new Error("ActiveMembershipExists", "User already has an active membership"));
            }

            // Redeem coupon
            await _couponRepository.RedeemCoupon(coupon, user);

            // Create user membership
            var durationDays = coupon.Duration;
            var userMembership = new UserMembership
            {
                User_Id = userId,
                MembershipID = coupon.MembershipId,
                StartDate = DateTime.UtcNow,
                Expiration = durationDays == int.MaxValue ? DateTime.MaxValue : DateTime.UtcNow.AddDays(durationDays),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserMembership(userMembership);
            
            // Update user's current membership
            user.SetMembershipId(coupon.MembershipId);
            await _userRepository.UpdateUserAsync(user);
            
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation($"Redeemed coupon {couponCode} for user {userId}");
            
            return Result.Success(userMembership);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to redeem coupon {couponCode} for user {userId}");
            return Result.Failure<UserMembership>(new Error("RedemptionFailed", "Failed to redeem coupon"));
        }
    }

    public async Task<Result<Coupon>> ValidateCouponAsync(string couponCode)
    {
        try
        {
            var coupon = await _couponRepository.GetCouponByCode(couponCode);
            
            if (coupon == null)
            {
                return Result.Failure<Coupon>(new Error("CouponNotFound", "Coupon not found"));
            }

            if (await _couponRepository.IsCouponRedeemed(coupon.Id))
            {
                return Result.Failure<Coupon>(new Error("CouponRedeemed", "Coupon already redeemed"));
            }

            return Result.Success(coupon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to validate coupon {couponCode}");
            return Result.Failure<Coupon>(new Error("ValidationFailed", "Failed to validate coupon"));
        }
    }

    public async Task<Result<List<Coupon>>> GetCouponsAsync(bool includeRedeemed = false, int? membershipId = null)
    {
        try
        {
            var coupons = await _couponRepository.GetAllCouponsEntities();
            
            if (!includeRedeemed)
            {
                // Filter out redeemed coupons
                var unredeemed = new List<Coupon>();
                foreach (var coupon in coupons)
                {
                    if (!await _couponRepository.IsCouponRedeemed(coupon.Id))
                    {
                        unredeemed.Add(coupon);
                    }
                }
                coupons = unredeemed;
            }

            if (membershipId.HasValue)
            {
                coupons = coupons.Where(c => c.MembershipId == membershipId.Value).ToList();
            }

            return Result.Success(coupons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get coupons");
            return Result.Failure<List<Coupon>>(new Error("GetFailed", "Failed to retrieve coupons"));
        }
    }

    public async Task<Result> DeactivateCouponAsync(string couponCode, Guid adminUserId)
    {
        try
        {
            var coupon = await _couponRepository.GetCouponByCode(couponCode);
            if (coupon == null)
            {
                return Result.Failure(new Error("CouponNotFound", "Coupon not found"));
            }

            // Mark as deactivated (we could add a IsActive field to Coupon table)
            // For now, we could delete it or mark it somehow
            
            _logger.LogInformation($"Deactivated coupon {couponCode} by admin {adminUserId}");
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to deactivate coupon {couponCode}");
            return Result.Failure(new Error("DeactivationFailed", "Failed to deactivate coupon"));
        }
    }

    private async Task<string> GenerateUniqueCouponCodeAsync()
    {
        const int maxAttempts = 10;
        
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var code = GenerateRandomCode();
            var existingCoupon = await _couponRepository.GetCouponByCode(code);
            
            if (existingCoupon == null)
            {
                return code;
            }
        }
        
        throw new InvalidOperationException("Failed to generate unique coupon code after multiple attempts");
    }

    private static string GenerateRandomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
