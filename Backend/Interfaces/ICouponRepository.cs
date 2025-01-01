using UGH.Domain.Entities;

namespace UGHApi.Interfaces;

public interface ICouponRepository
{
    Task AddCoupon(Coupon coupon);
    Task UpdateCoupon(Coupon updatedCoupon);
    Task<bool> IsCouponExists(string couponCode);
    Task<List<Coupon>> GetAllCoupons();
    Task DeleteCoupon(int couponId);
    Task RedeemCoupon(string couponCode, Guid userId);
}
