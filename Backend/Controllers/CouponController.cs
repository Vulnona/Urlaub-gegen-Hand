using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using UGHApi.Models;
using UGHApi.Services;


namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly CouponService _couponService;
        private readonly ILogger<CouponController> _logger;


        public CouponController(UghContext context, CouponService couponService, ILogger<CouponController> logger)
        {
            _context = context;
            _couponService = couponService;
            _logger = logger;
        }

        #region Coupon-generation-by-admin
        [HttpPost("admin/add-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCoupon(Coupon coupon)
        {
            try
            {
                var existingCoupon = await _context.coupons.FirstOrDefaultAsync(x => x.Code.Equals(coupon.Code));

                if (existingCoupon != null)
                {
                    return Conflict("Coupon code already exists.");
                }

                await _couponService.AddCoupon(coupon);
                return Ok("Coupon inserted successfully.");
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("admin/update-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCoupon(Coupon updatedCoupon)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await _couponService.UpdateCoupon(updatedCoupon);
                return Ok("Coupon updated successfully.");
            }
            catch (CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("admin/get-all-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCoupon()
        {
            try
            {
            var coupons = await _couponService.GetAllcoupons();
            if (coupons.IsNullOrEmpty()) return NotFound();
            return Ok(coupons);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("admin/delete-coupon/{couponId}")]
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
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("coupon/redeem")]
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
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
