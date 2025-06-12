using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Extensions;
using UGHApi.Interfaces;

namespace UGHApi.Applications.Coupons;

public class AddCouponCommandHandler : IRequestHandler<AddCouponCommand, Result>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly ILogger<AddCouponCommandHandler> _logger;

    public AddCouponCommandHandler(
        ICouponRepository couponRepository,
        IMembershipRepository membershipRepository,
        ILogger<AddCouponCommandHandler> logger
    )
    {
        _couponRepository = couponRepository;
        _membershipRepository = membershipRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(AddCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var membership = await _membershipRepository.GetMembershipByIdAsync(request.MembershipId);

            if (membership == null)
            {
                return Result.Failure<string>(
                    Errors.General.InvalidOperation("Membership not found.")
                );
            }

            var newCoupon = new Coupon
            {
                Code = Ulid.NewUlid().ToString(),
                Name = "Admin Issued Coupon",
                Description = string.Empty,
                CreatedBy = request.UserId,
                MembershipId = membership.MembershipID,
                Duration = membership.DurationDays.ToCouponDuration(),
            };

            await _couponRepository.AddCoupon(newCoupon);

            return Result.Success(newCoupon.Code);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<string>(
                Errors.General.InvalidOperation($"Internal server error: {ex.Message}")
            );
        }
    }
}

// using MediatR;
// using UGH.Domain.Core;
// using UGH.Domain.Entities;
// using UGHApi.Interfaces;

// namespace UGHApi.Applications.Coupons;

// public class AddCouponCommandHandler : IRequestHandler<AddCouponCommand, Result>
// {
//     private readonly ICouponRepository _couponRepository;
//     private readonly ILogger<AddCouponCommandHandler> _logger;

//     public AddCouponCommandHandler(
//         ICouponRepository couponRepository,
//         ILogger<AddCouponCommandHandler> logger
//     )
//     {
//         _couponRepository = couponRepository;
//         _logger = logger;
//     }

//     public async Task<Result> Handle(AddCouponCommand request, CancellationToken cancellationToken)
//     {
//         try
//         {
//             var newCoupon = new Coupon
//             {
//                 Code = Ulid.NewUlid().ToString(),
//                 Name = "Admin Issued Coupon",
//                 Description = string.Empty,
//                 CreatedBy = request.UserId,
//                 MembershipId = 1, //Statically stored for now.
//                 Duration = UGH_Enums.CouponDuration.ThreeMonths,
//             };

//             await _couponRepository.AddCoupon(newCoupon);

//             return Result.Success(newCoupon.Code);
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
//             return Result.Failure<string>(
//                 Errors.General.InvalidOperation($"Internal server error: {ex.Message}")
//             );
//         }
//     }
// }
