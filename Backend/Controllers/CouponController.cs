using MailKit.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;
using UGHApi.Applications.Admin;
using UGHApi.Applications.Coupons;
using UGHApi.Interfaces;
using UGHApi.Models;
using UGHApi.Repositories;
using UGHApi.Services.HtmlTemplate;
using UGHApi.Services.UserProvider;
using static OpenQA.Selenium.PrintOptions;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly EmailService _emailService;
        private readonly HtmlTemplateService _htmlTemplateService;
        private readonly ICouponRepository _couponRepository;
        private readonly IUserProvider _userProvider;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CouponController> _logger;

        public CouponController(
            IMediator mediator,
            ICouponRepository couponRepository,
            HtmlTemplateService htmlTemplateService,
            IUserProvider userProvider,
            IUserRepository userRepository,
            ILogger<CouponController> logger,
            EmailService emailService
        )
        {
            _mediator = mediator;
            _couponRepository = couponRepository;
            _htmlTemplateService = htmlTemplateService;
            _userProvider = userProvider;
            _userRepository = userRepository;
            _logger = logger;
            _emailService = emailService;
        }

        #region Coupon-generation-by-admin
        [HttpPost("admin/add-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCoupon()
        {
            try
            {
                var result = await _mediator.Send(new AddCouponCommand());

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("admin/send-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendCoupon([FromBody] SendCouponRequest reuest)
        {
            try
            {
                var result = await _mediator.Send(
                    new SendCouponQuery(reuest.UserId, reuest.CouponCode)
                );

                return Ok(result);
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                await _couponRepository.UpdateCoupon(updatedCoupon);
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
                var coupons = await _couponRepository.GetAllCoupons();
                if (coupons.IsNullOrEmpty())
                    return NotFound();
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
                await _couponRepository.DeleteCoupon(couponId);
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
        public async Task<IActionResult> RedeemCoupon([FromBody] RedeemCouponRequest request)
        {
            try
            {
                var userId = _userProvider.UserId;
                var result = await _mediator.Send(
                    new RedeemMembershipCommand(userId, 1, request.CouponCode)
                );
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
