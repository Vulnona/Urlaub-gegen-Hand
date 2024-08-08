using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly couponservice _couponservice;


        public CouponController(UghContext context, couponservice couponservice)
        {
            _context = context;
            _couponservice = couponservice;
        }

        #region Coupon

       

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

                await _couponservice.AddCoupon(coupon);
                return Ok("Coupon inserted successfully.");
            }
            catch (Exception)
            {
                // Log the exception if needed
                return BadRequest("An error occurred while processing the request.");
            }
        }


        [HttpPost("admin/update-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCoupon(Coupon updatedCoupon)
        {
            try
            {
                await _couponservice.UpdateCoupon(updatedCoupon);
                return Ok("Coupon updated successfully.");
            }
            catch (CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (Exception)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while updating the coupon.");
            }
        }

        [HttpGet("admin/get-all-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCoupon()
        {
            var coupons = await _couponservice.GetAllcoupons();
            return Ok(coupons);
        }

        [HttpDelete("admin/delete-coupon/{couponId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            try
            {
                await _couponservice.DeleteCoupon(couponId);
                return Ok("Coupon deleted successfully.");
            }
            catch (CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (Exception)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while deleting the coupon.");
            }
        }

        [HttpPost("coupon/redeem")]
        [Authorize]
        public async Task<IActionResult> RedeemCoupon(string couponCode)
        {
            try
            {
                var result = await _couponservice.RedeemCoupon(couponCode, User);
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
            catch (Exception)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while redeeming the coupon.");
            }
        }
        #endregion
    }
}
