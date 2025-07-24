using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;
using UGHApi.Applications.Coupons;
using UGHApi.Services.HtmlTemplate;

public class SendCouponQueryHandler : IRequestHandler<SendCouponQuery, Result>
{
#pragma warning disable CS4014
    private readonly IUserRepository _userRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly HtmlTemplateService _htmlTemplateService;
    private readonly EmailService _emailService;
    private readonly ILogger<SendCouponQueryHandler> _logger;

    public SendCouponQueryHandler(
        IUserRepository userRepository,
        ICouponRepository couponRepository,
        HtmlTemplateService htmlTemplateService,
        EmailService emailSender,
        ILogger<SendCouponQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _couponRepository = couponRepository;
        _htmlTemplateService = htmlTemplateService;
        _emailService = emailSender;
        _logger = logger;
    }

    public async Task<Result> Handle(SendCouponQuery request, CancellationToken cancellationToken)
    {
        var requestCode = request.CouponCode.Trim();
        var couponEntity = await _couponRepository.GetCouponByCodeAsync(requestCode);

        if (couponEntity == null)
        {
            return Result.Failure(new Error("CouponNotExist", "Coupon does not exists."));
        }

        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(Errors.General.NotFound("User", request.UserId));

        // Update coupon with email tracking before sending
        try
        {
            var htmlTemplate = _htmlTemplateService.GetCouponReceivedDetails(
                requestCode,
                $"{user.FirstName} {user.LastName}".Trim()
            );

            var emailSent = await _emailService.SendEmailAsync(
                user.Email_Address,
                htmlTemplate.Subject,
                htmlTemplate.BodyHtml
            );

            if (emailSent)
            {
                // Update coupon with email tracking information
                couponEntity.IsEmailSent = true;
                couponEntity.EmailSentDate = DateTime.Now;
                couponEntity.EmailSentTo = user.Email_Address;
                await _couponRepository.UpdateCouponAsync(couponEntity);
                
                _logger.LogInformation($"Coupon {requestCode} successfully sent to {user.Email_Address}");
                return Result.Success("Coupon sent successfully.");
            }
            else
            {
                _logger.LogError($"Failed to send coupon {requestCode} to {user.Email_Address}");
                return Result.Failure(new Error("EmailSendFailed", "Failed to send email."));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email to {user.Email_Address}: {ex.Message}");
            return Result.Failure(new Error("EmailSendError", "An error occurred while sending email."));
        }
    }
}
