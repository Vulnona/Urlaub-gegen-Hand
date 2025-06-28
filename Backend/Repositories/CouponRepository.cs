using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGHApi.Extensions;
using UGHApi.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly Ugh_Context _context;

    public CouponRepository(Ugh_Context context)
    {
        _context = context;
    }

    #region coupons
    public async Task<Coupon> AddCoupon(Coupon coupon)
    {
        coupon.CreatedDate = DateTime.UtcNow;
        _context.coupons.Add(coupon);
        await _context.SaveChangesAsync();
        return coupon;
    }

    public async Task UpdateCoupon(Coupon updatedCoupon)
    {
        var existingCoupon = await _context.coupons.FindAsync(updatedCoupon.Id);
        if (existingCoupon == null)
        {
            throw new CouponNotFoundException();
        }

        existingCoupon.Code = updatedCoupon.Code;
        existingCoupon.Name = updatedCoupon.Name;
        existingCoupon.Description = updatedCoupon.Description;
        existingCoupon.CreatedDate = DateTime.UtcNow;

        _context.Entry(existingCoupon).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedList<CouponDto>> GetAllCoupons(int pageNumber, int pageSize)
    {
        int totalCount = await _context.coupons.CountAsync();

        var couponsWithRedemptions = await _context
            .coupons.Include(c => c.CreatedByUser)
            .Include(c => c.Redemption)
            .OrderByDescending(c => c.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var couponDtos = couponsWithRedemptions
            .Select(coupon =>
            {
                var couponDto = coupon.Adapt<CouponDto>();
                couponDto.IsRedeemed = coupon.Redemption != null;
                couponDto.Duration = coupon.Duration.ToDays();
                if (couponDto.IsRedeemed)
                {
                couponDto.RedeemedBy = coupon.Redemption?.UserEmail ?? null;
                }
                return couponDto;   
            })
            .ToList();

        return PaginatedList<CouponDto>.Create(couponDtos, totalCount, pageNumber, pageSize);
    }

    public async Task<bool> IsCouponExists(string couponCode)
    {
        if (await _context.coupons.FirstOrDefaultAsync(c => c.Code == couponCode) is Coupon)
            return true;
        else
        {
            return false;
        }
    }

    public async Task<bool> IsCouponRedeemed(int couponId)
    {
        if (
            await _context.redemptions.FirstOrDefaultAsync(c => c.CouponId == couponId)
            is Redemption
        )
            return true;
        else
        {
            return false;
        }
    }

    public async Task<Coupon> GetCouponById(int couponId)
    {
        var coupon = await _context
            .coupons.Include(c => c.CreatedByUser)
            .FirstOrDefaultAsync(c => c.Id == couponId);

        if (coupon == null)
        {
            throw new CouponNotFoundException();
        }

        return coupon;
    }

    public async Task DeleteCoupon(int couponId)
    {
        var coupon = await _context.coupons.FindAsync(couponId);
        if (coupon == null)
        {
            throw new CouponNotFoundException();
        }

        _context.coupons.Remove(coupon);
        await _context.SaveChangesAsync();
    }

    public async Task RedeemCoupon(Coupon coupon, User user)
    {
        try
        {
            var currentDate = DateTime.UtcNow;

            var redemption = new Redemption
            {
                CouponId = coupon.Id,
                UserId = user.User_Id,
                UserEmail = user.Email_Address,
                RedeemedDate = currentDate,
            };

            _context.redemptions.Add(redemption);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while redeeming the coupon.", ex);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Coupon> GetCouponByCode(string couponCode)
    {
        return await _context
            .coupons.Include(c => c.Membership)
            .FirstOrDefaultAsync(c => c.Code == couponCode.ToUpper().Trim());
    }

    public class CouponNotFoundException : Exception
    {
        public CouponNotFoundException()
            : base("Coupon not found.") { }
    }

    public class CouponRedeemException : Exception
    {
        public CouponRedeemException(string message)
            : base(message) { }
    }
}
    #endregion


