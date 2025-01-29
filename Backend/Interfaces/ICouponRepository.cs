using UGH.Domain.Entities;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Interfaces;

public interface ICouponRepository
{
    Task<Coupon> AddCoupon(Coupon coupon);
    Task UpdateCoupon(Coupon updatedCoupon);
    Task<Coupon> GetCouponById(int couponId);
    Task<Coupon> GetCouponByCode(string couponCode);
    Task<bool> IsCouponExists(string couponCode);
    Task<bool> IsCouponRedeemed(int couponId);
    Task<PaginatedList<CouponDto>> GetAllCoupons(int pageNumber, int pageSize);
    Task DeleteCoupon(int couponId);
    Task RedeemCoupon(Coupon coupon, Guid userId);
    Task SaveChangesAsync();
}
