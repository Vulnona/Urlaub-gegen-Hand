using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Interfaces;
using UGHApi.Repositories;

namespace UGHApi.Applications.Admin;

public class RedeemMembershipCommandHandler : IRequestHandler<RedeemMembershipCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IMembershipRepository _membershipRepository;

    public RedeemMembershipCommandHandler(
        IUserRepository userRepository,
        IMembershipRepository membershipRepository,
        ICouponRepository couponRepository
    )
    {
        _userRepository = userRepository;
        _membershipRepository = membershipRepository;
        _couponRepository = couponRepository;
    }

    public async Task<Result> Handle(
        RedeemMembershipCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user = await _userRepository.GetUserForMembershipByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure(
                    new Error("UserNotFound", "The specified user does not exist.")
                );
            }

            var existingActiveMembership = user.UserMemberships.FirstOrDefault(um =>
                um.IsMembershipActive
            );
            if (existingActiveMembership != null)
            {
                return Result.Failure(
                    new Error("ActiveMembershipExists", "The user already has active memberships.")
                );
            }

            await _couponRepository.RedeemCoupon(request.CouponCode, request.UserId);

            var membership = await _membershipRepository.GetMembershipByIdAsync(
                request.MembershipId
            );
            if (membership == null)
            {
                return Result.Failure(
                    new Error("MembershipNotFound", "The specified membership does not exist.")
                );
            }

            if (existingActiveMembership == null)
            {
                user.SetMembershipId(membership.MembershipID);
                await _userRepository.UpdateUserAsync(user);
            }

            var userMembership = new UserMembership
            {
                User_Id = request.UserId,
                MembershipID = request.MembershipId,
                StartDate = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(membership.DurationDays),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _userRepository.AddUserMembership(userMembership);
            await _userRepository.SaveChangesAsync();

            return Result.Success("Coupon Redeemed Successfully");
        }
        catch (CouponNotFoundException ex)
        {
            return Result.Failure(new Error("CouponNotFound", ex.Message));
        }
        catch (CouponRedeemException ex)
        {
            return Result.Failure(new Error("CouponRedemptionFailed", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("InternalError", "An unexpected error occurred."));
        }
    }
}
