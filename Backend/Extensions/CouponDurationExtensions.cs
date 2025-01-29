using static UGH.Domain.Core.UGH_Enums;

namespace UGHApi.Extensions;

public static class CouponDurationExtensions
{
    public static int ToDays(this CouponDuration duration)
    {
        return duration switch
        {
            CouponDuration.ThreeMonths => 3 * 30,
            CouponDuration.SixMonths => 6 * 30,
            CouponDuration.OneYear => 365,
            CouponDuration.TwoYears => 2 * 365,
            _ => throw new ArgumentOutOfRangeException(
                nameof(duration),
                "Invalid coupon duration."
            ),
        };
    }
}
