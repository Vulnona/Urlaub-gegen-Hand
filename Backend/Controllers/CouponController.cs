using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UGHApi.Models;
using UGHApi.Services;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly CouponService _couponService;


        public CouponController(UghContext context, CouponService couponService)
        {
            _context = context;
            _couponService = couponService;
        }

        #region Coupon
        [HttpPost("Admin/AddCoupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCoupon(Coupon coupon)
        {
            try
            {
                var existingCoupon = await _context.Coupons.FirstOrDefaultAsync(x => x.Code.Equals(coupon.Code));

                if (existingCoupon != null)
                {
                    return Conflict("Coupon code already exists.");
                }

                await _couponService.AddCoupon(coupon);
                return Ok("Coupon inserted successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return BadRequest("An error occurred while processing the request.");
            }
        }


        [HttpPost("Admin/UpdateCoupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCoupon(Coupon updatedCoupon)
        {
            try
            {
                await _couponService.UpdateCoupon(updatedCoupon);
                return Ok("Coupon updated successfully.");
            }
            catch (CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while updating the coupon.");
            }
        }

        [HttpGet("Admin/GetAllCoupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCoupon()
        {
            var coupons = await _couponService.GetAllCoupons();
            return Ok(coupons);
        }
        [HttpDelete("Admin/deleteCoupon/{couponId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            try
            {
                await _couponService.DeleteCoupon(couponId);
                return Ok("Coupon deleted successfully.");
            }
            catch (CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while deleting the coupon.");
            }
        }
        [HttpPost("[Controller]/redeem")]
        [Authorize]
        public async Task<IActionResult> RedeemCoupon(string couponCode)
        {
            try
            {
                var result = await _couponService.RedeemCoupon(couponCode, User);
                return Ok(result);
            }
            catch (CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (CouponRedeemException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while redeeming the coupon.");
            }
        }
        #endregion
    }
}
