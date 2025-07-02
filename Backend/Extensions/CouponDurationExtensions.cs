using static UGH.Domain.Core.UGH_Enums;

namespace UGHApi.Extensions;

public static class CouponDurationExtensions
{
    public static int ToDays(this CouponDuration duration)
    {
        return duration switch
        {
            CouponDuration.EinJahr => 365,
            CouponDuration.ZweiJahre => 730,
            CouponDuration.DreiJahre => 1095,
            CouponDuration.Keiner => 0,
            CouponDuration.Lebenslang => int.MaxValue,
            _ => throw new ArgumentOutOfRangeException(
                nameof(duration),
                "Invalid coupon duration."
            ),
        };
    }

    public static CouponDuration ToCouponDuration(this int days)
    {
        return days switch
        {
            <= 0 => CouponDuration.Keiner,
            <= 365 => CouponDuration.EinJahr,
            <= 730 => CouponDuration.ZweiJahre,
            <= 1095 => CouponDuration.DreiJahre,
            _ => CouponDuration.Lebenslang,
        };
    }
}
