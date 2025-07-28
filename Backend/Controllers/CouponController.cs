using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using UGH.Domain.Entities;
using UGHApi.Applications.Admin;
using UGHApi.Applications.Coupons;
using UGHApi.Applications.ShopItems;
using UGH.Domain.Interfaces;
using UGHApi.Models;
using UGHApi.ViewModels;
using UGHApi.Repositories;
using UGHApi.Services.UserProvider;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
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
        [HttpPost("add-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCoupon([FromBody] AddCouponRequest request)
        {
            try
            {
                _logger.LogInformation($"=== DEBUG: AddCoupon called ===");
                _logger.LogInformation($"Request: {System.Text.Json.JsonSerializer.Serialize(request)}");
                
                var userId = _userProvider.UserId;
                var result = await _mediator.Send(new AddCouponCommand(userId, request.MembershipId));

                _logger.LogInformation($"AddCoupon result: {System.Text.Json.JsonSerializer.Serialize(result)}");
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

        [HttpPost("coupon/create-and-send")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAndSendCoupon([FromBody] CreateAndSendCouponRequest request)
        {
            try
            {
                var result = await _mediator.Send(
                    new CreateAndSendCouponCommand(request.Email, request.MembershipId, request.Name)
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("coupon/send-existing")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendExistingCoupon([FromBody] SendExistingCouponRequest request)
        {
            try
            {
                var result = await _mediator.Send(
                    new SendExistingCouponCommand(request.CouponId)
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
                _logger.LogInformation($"=== DEBUG: GetAllCoupon called ===");
                _logger.LogInformation($"Request URI: {Request.Path}{Request.QueryString}");
                _logger.LogInformation($"Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");
                _logger.LogInformation($"pageNumber: {pageNumber}, pageSize: {pageSize}");
                
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var coupons = await _couponRepository.GetAllCoupons(pageNumber, pageSize);

                if (coupons.Items.IsNullOrEmpty())
                {
                    _logger.LogWarning("No coupons found - returning empty list");
                    return Ok(new
                    {
                        Items = new List<CouponDto>(),
                        TotalCount = 0,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalPages = 0
                    });
                }

                var response = new
                {
                    coupons.Items,
                    coupons.TotalCount,
                    coupons.PageNumber,
                    coupons.PageSize,
                    coupons.TotalPages,
                };

                _logger.LogInformation($"Returning {coupons.Items.Count()} coupons");
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
            _logger.LogInformation($"Coupon wird eingel�st userid:{_userProvider.UserId}, code:{request.CouponCode}");
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

        [EnableRateLimiting("StripeWebhookPolicy")]
        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            if (string.IsNullOrEmpty(stripeSignature))
            {
                return NotFound("Stripe-Signature not found");
            }

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
