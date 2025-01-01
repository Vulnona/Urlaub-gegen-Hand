using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGHApi.Interfaces;

namespace UGHApi.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly Ugh_Context _context;

    public CouponRepository(Ugh_Context context)
    {
        _context = context;
    }

    #region coupons
    public async Task AddCoupon(Coupon coupon)
    {
        coupon.CreatedDate = DateTime.UtcNow;
        _context.coupons.Add(coupon);
        await _context.SaveChangesAsync();
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
        existingCoupon.EndDate = updatedCoupon.EndDate;
        existingCoupon.StartDate = updatedCoupon.StartDate;

        _context.Entry(existingCoupon).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Coupon>> GetAllCoupons()
    {
        return await _context.coupons.ToListAsync();
    }

    public async Task<bool> IsCouponExists(string couponCode)
    {
        if (
            await _context.coupons.FirstOrDefaultAsync(c =>
                c.Code == couponCode && c.StartDate == null
            ) is Coupon
        )
            return true;
        else
        {
            return false;
        }
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

    public async Task RedeemCoupon(string couponCode, Guid userId)
    {
        try
        {
            var coupon = await _context.coupons.FirstOrDefaultAsync(c =>
                c.Code == couponCode.Trim()
            );

            if (coupon == null)
            {
                throw new CouponNotFoundException();
            }

            if (coupon.StartDate?.Date != null)
            {
                throw new CouponRedeemException("Coupon has already been redeemed.");
            }

            var currentDate = DateTime.UtcNow;
            var couponExpiry = currentDate.AddDays(365);

            coupon.StartDate = currentDate;
            coupon.EndDate = couponExpiry;

            _context.Update(coupon);

            var redemption = new Redemption
            {
                CouponId = coupon.Id,
                UserId = userId,
                RedeemedDate = currentDate,
            };

            _context.redemptions.Add(redemption);
            await _context.SaveChangesAsync();
        }
        catch (CouponNotFoundException)
        {
            throw new CouponNotFoundException();
        }
        catch (CouponRedeemException)
        {
            throw new CouponRedeemException("Coupon redemption failed!");
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while redeeming the coupon.", ex);
        }
    }
}

public class CouponNotFoundException : Exception
{
    public CouponNotFoundException()
        : base("Coupon not found.") { }
}

public class UserNotFoundException : Exception
{
    public UserNotFoundException()
        : base("User not found.") { }
}

public class CouponRedeemException : Exception
{
    public CouponRedeemException(string message)
        : base(message) { }
}
    #endregion
