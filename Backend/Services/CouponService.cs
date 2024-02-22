using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using UGHApi.Models;
using UGHModels;
namespace UGHApi.Services
{
    public class CouponService
    {
        private readonly UghContext _context;

        public CouponService(UghContext context)
        {
            _context = context;
        }
        public async Task AddCoupon(Coupon coupon)
        {
            coupon.CreatedDate = DateTime.Now;
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCoupon(Coupon updatedCoupon)
        {
            var existingCoupon = await _context.Coupons.FindAsync(updatedCoupon.Id);
            if (existingCoupon == null)
            {
                throw new CouponNotFoundException();
            }

            existingCoupon.Code = updatedCoupon.Code;
            existingCoupon.Name = updatedCoupon.Name;
            existingCoupon.Description = updatedCoupon.Description;
            existingCoupon.CreatedDate = DateTime.Now;
            existingCoupon.EndDate = updatedCoupon.EndDate;
            existingCoupon.StartDate = updatedCoupon.StartDate;
            existingCoupon.DiscountAmount = updatedCoupon.DiscountAmount;

            _context.Entry(existingCoupon).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Coupon>> GetAllCoupons()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task DeleteCoupon(int couponId)
        {
            var coupon = await _context.Coupons.FindAsync(couponId);
            if (coupon == null)
            {
                throw new CouponNotFoundException();
            }

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task<string> RedeemCoupon(string couponCode, ClaimsPrincipal user)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == couponCode);
            if (coupon == null)
            {
                throw new CouponNotFoundException();
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRecord = await _context.Users.FirstOrDefaultAsync(u => u.Email_Adress == userId);
            if (userRecord == null)
            {
                throw new UserNotFoundException();
            }

            var currentDate = DateTime.UtcNow.Date;

            if (currentDate < coupon.StartDate.Date || currentDate > coupon.EndDate.Date)
            {
                throw new CouponRedeemException("Coupon is not valid at this time.");
            }

            if (coupon.EndDate.Date < currentDate)
            {
                throw new CouponRedeemException("Coupon has expired.");
            }

            var redemption = new Redemption
            {
                CouponId = coupon.Id,
                UserId = userRecord.User_Id,
                RedeemedDate = currentDate
            };

            _context.Redemptions.Add(redemption);
            await _context.SaveChangesAsync();

            return "Coupon redeemed successfully.";
        }
    }
    public class CouponNotFoundException : Exception
    {
        public CouponNotFoundException() : base("Coupon not found.") { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.") { }
    }

    public class CouponRedeemException : Exception
    {
        public CouponRedeemException(string message) : base(message) { }
    }
}
