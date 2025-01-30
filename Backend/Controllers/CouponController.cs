using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UGH.Domain.Entities;
using UGHApi.Applications.Admin;
using UGHApi.Applications.Coupons;
using UGHApi.Applications.ShopItems;
using UGHApi.Interfaces;
using UGHApi.Models;
using UGHApi.Repositories;
using UGHApi.Services.UserProvider;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CouponController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICouponRepository _couponRepository;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<CouponController> _logger;

        public CouponController(
            IMediator mediator,
            ICouponRepository couponRepository,
            IUserProvider userProvider,
            ILogger<CouponController> logger
        )
        {
            _mediator = mediator;
            _couponRepository = couponRepository;
            _userProvider = userProvider;
            _logger = logger;
        }

        #region Coupon-generation-by-admin
        [HttpPost("coupon/add-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCoupon()
        {
            try
            {
                var userId = _userProvider.UserId;
                var result = await _mediator.Send(new AddCouponCommand(userId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("coupon/send-coupon")]
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

        [HttpPost("coupon/update-coupon")]
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
            catch (Repositories.CouponRepository.CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("coupon/get-all-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCoupon(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var coupons = await _couponRepository.GetAllCoupons(pageNumber, pageSize);

                if (coupons.Items.IsNullOrEmpty())
                {
                    return NotFound("No coupons found.");
                }

                var response = new
                {
                    coupons.Items,
                    coupons.TotalCount,
                    coupons.PageNumber,
                    coupons.PageSize,
                    coupons.TotalPages,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("coupon/delete-coupon/{couponId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            try
            {
                await _couponRepository.DeleteCoupon(couponId);
                return Ok("Coupon deleted successfully.");
            }
            catch (CouponRepository.CouponNotFoundException)
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
                    new RedeemMembershipCommand(userId, request.CouponCode)
                );
                return Ok(result);
            }
            catch (CouponRepository.CouponNotFoundException)
            {
                return NotFound("Coupon not found.");
            }
            catch (CouponRepository.CouponRedeemException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent(
            [FromBody] CreatePaymentIntentRequest request
        )
        {
            try
            {
                var userId = _userProvider.UserId;
                var command = new CreatePaymentIntentCommand
                {
                    UserId = userId,
                    ShopItemId = request.ShopItemId,
                };

                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            var command = new PaymentSucceededWebhookCommand
            {
                JsonPayload = json,
                StripeSignature = stripeSignature,
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        public class PaymentEvent
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public PaymentObject @object { get; set; }
        }

        public class PaymentObject
        {
            public string id { get; set; }
        }

        #endregion
    }
}
