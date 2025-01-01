using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Interfaces;

namespace UGHApi.Applications.Coupons;

public class AddCouponCommandHandler : IRequestHandler<AddCouponCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ILogger<AddCouponCommandHandler> _logger;

    public AddCouponCommandHandler(
        IUserRepository userRepository,
        ICouponRepository couponRepository,
        ILogger<AddCouponCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _couponRepository = couponRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(AddCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newCoupon = new Coupon
            {
                Code = Ulid.NewUlid().ToString(),
                Name = "Membership Coupon",
                Description = string.Empty,
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
