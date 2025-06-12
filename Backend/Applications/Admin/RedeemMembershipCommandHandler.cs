using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Interfaces;

namespace UGHApi.Applications.Admin;

public class RedeemMembershipCommandHandler : IRequestHandler<RedeemMembershipCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ICouponRepository _couponRepository;

    public RedeemMembershipCommandHandler(
        IUserRepository userRepository,
        ICouponRepository couponRepository
    )
    {
        _userRepository = userRepository;
        _couponRepository = couponRepository;
    }

    public async Task<Result> Handle(
        RedeemMembershipCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var coupon = await _couponRepository.GetCouponByCode(request.CouponCode);

            if (coupon == null)
            {
                return Result.Failure(
                    new Error("CouponNotFound", "The specified coupon code does not exist.")
                );
            }

            if (await _couponRepository.IsCouponRedeemed(coupon.Id))
            {
                return Result.Failure(
                    new Error(
                        "CouponAlreadyRedeemed",
                        "The specified coupon code has already been redeemed."
                    )
                );
            }

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
                    new Error(
                        "ActiveMembershipExists",
                        "The user already has an active membership."
                    )
                );
            }

            await _couponRepository.RedeemCoupon(coupon, user);

            user.SetMembershipId(coupon.MembershipId);
            await _userRepository.UpdateUserAsync(user);

            var userMembership = new UserMembership
            {
                User_Id = user.User_Id,
                MembershipID = coupon.MembershipId,
                StartDate = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(coupon.Membership.DurationDays),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _userRepository.AddUserMembership(userMembership);
            await _userRepository.SaveChangesAsync();

            return Result.Success("Coupon Redeemed Successfully");
        }
        catch (Repositories.CouponRepository.CouponNotFoundException ex)
        {
            return Result.Failure(new Error("CouponNotFound", ex.Message));
        }
        catch (Repositories.CouponRepository.CouponRedeemException ex)
        {
            return Result.Failure(new Error("CouponRedemptionFailed", ex.Message)); // Provide more details here
        }
        catch (Exception ex)
        {
            return Result.Failure(
                new Error("InternalError", $"An unexpected error occurred: {ex.Message}.")
            );
        }
    }
}

// using MediatR;
// using UGH.Domain.Core;
// using UGH.Domain.Entities;
// using UGH.Domain.Interfaces;
// using UGHApi.Interfaces;

// namespace UGHApi.Applications.Admin;

// public class RedeemMembershipCommandHandler : IRequestHandler<RedeemMembershipCommand, Result>
// {
//     private readonly IUserRepository _userRepository;
//     private readonly ICouponRepository _couponRepository;

//     public RedeemMembershipCommandHandler(
//         IUserRepository userRepository,
//         ICouponRepository couponRepository
//     )
//     {
//         _userRepository = userRepository;
//         _couponRepository = couponRepository;
//     }

//     public async Task<Result> Handle(
//         RedeemMembershipCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         try
//         {
//             var coupon = await _couponRepository.GetCouponByCode(request.CouponCode);

//             if (coupon == null)
//             {
//                 return Result.Failure(
//                     new Error("CouponNotFound", "The specified coupon code does not exist.")
//                 );
//             }

//             if (await _couponRepository.IsCouponRedeemed(coupon.Id))
//             {
//                 return Result.Failure(
//                     new Error(
//                         "CouponAlreadyRedeemed",
//                         "The specified coupon code has already been redeemed."
//                     )
//                 );
//             }

//             var user = await _userRepository.GetUserForMembershipByIdAsync(request.UserId);

//             if (user == null)
//             {
//                 return Result.Failure(
//                     new Error("UserNotFound", "The specified user does not exist.")
//                 );
//             }

//             var existingActiveMembership = user.UserMemberships.FirstOrDefault(um =>
//                 um.IsMembershipActive
//             );

//             if (existingActiveMembership != null)
//             {
//                 return Result.Failure(
//                     new Error(
//                         "ActiveMembershipExists",
//                         "The user already has an active membership."
//                     )
//                 );
//             }

//             await _couponRepository.RedeemCoupon(coupon, request.UserId);

//             user.SetMembershipId(coupon.MembershipId);
//             await _userRepository.UpdateUserAsync(user);

//             var userMembership = new UserMembership
//             {
//                 User_Id = user.User_Id,
//                 MembershipID = coupon.MembershipId,
//                 StartDate = DateTime.UtcNow,
//                 Expiration = DateTime.UtcNow.AddDays(coupon.Membership.DurationDays),
//                 CreatedAt = DateTime.UtcNow,
//                 UpdatedAt = DateTime.UtcNow,
//             };

//             await _userRepository.AddUserMembership(userMembership);
//             await _userRepository.SaveChangesAsync();

//             return Result.Success("Coupon Redeemed Successfully");
//         }
//         catch (Repositories.CouponRepository.CouponNotFoundException ex)
//         {
//             return Result.Failure(new Error("CouponNotFound", ex.Message));
//         }
//         catch (Repositories.CouponRepository.CouponRedeemException ex)
//         {
//             return Result.Failure(new Error("CouponRedemptionFailed", ex.Message)); // Provide more details here
//         }
//         catch (Exception ex)
//         {
//             return Result.Failure(
//                 new Error("InternalError", $"An unexpected error occurred: {ex.Message}.")
//             );
//         }
//     }
// }
