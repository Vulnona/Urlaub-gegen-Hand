using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGHApi.Services.HtmlTemplate;
using UGH.Infrastructure.Services;

namespace UGHApi.Applications.Coupons;

public class SendExistingCouponCommandHandler : IRequestHandler<SendExistingCouponCommand, Result>
{
    private readonly ICouponRepository _couponRepository;
    private readonly HtmlTemplateService _htmlTemplateService;
    private readonly EmailService _emailService;
    private readonly ILogger<SendExistingCouponCommandHandler> _logger;

    public SendExistingCouponCommandHandler(
        ICouponRepository couponRepository,
        HtmlTemplateService htmlTemplateService,
        EmailService emailService,
        ILogger<SendExistingCouponCommandHandler> logger
    )
    {
        _couponRepository = couponRepository;
        _htmlTemplateService = htmlTemplateService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(SendExistingCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get coupon with user information
            var coupon = await _couponRepository.GetCouponById(request.CouponId);
            if (coupon == null)
            {
                return Result.Failure(new Error("CouponNotFound", "Coupon nicht gefunden."));
            }

            // Check if coupon is already redeemed
            if (coupon.Redemption != null)
            {
                return Result.Failure(new Error("CouponRedeemed", "Coupon wurde bereits eingelöst."));
            }

            // Check if email was already sent
            if (coupon.IsEmailSent)
            {
                return Result.Failure(new Error("EmailAlreadySent", "E-Mail wurde bereits gesendet."));
            }

            // Send email
            var htmlTemplate = _htmlTemplateService.GetCouponReceivedDetails(
                coupon.Code,
                $"{coupon.CreatedByUser.FirstName} {coupon.CreatedByUser.LastName}".Trim(),
                coupon.Membership?.Name ?? "Unbekannt",
                (int)Math.Round((coupon.Membership?.DurationDays ?? 0) / 30.0)
            );

            var emailSent = await _emailService.SendEmailAsync(
                coupon.CreatedByUser.Email_Address,
                htmlTemplate.Subject,
                htmlTemplate.BodyHtml
            );

            if (emailSent)
            {
                // Update coupon with email tracking information
                coupon.IsEmailSent = true;
                coupon.EmailSentDate = DateTime.Now;
                coupon.EmailSentTo = coupon.CreatedByUser.Email_Address;
                await _couponRepository.UpdateCouponAsync(coupon);

                _logger.LogInformation($"Coupon {coupon.Code} sent to {coupon.CreatedByUser.Email_Address}");
                return Result.Success("E-Mail erfolgreich gesendet.");
            }
            else
            {
                _logger.LogError($"Failed to send coupon {coupon.Code} to {coupon.CreatedByUser.Email_Address}");
                return Result.Failure(new Error("EmailSendFailed", "E-Mail konnte nicht gesendet werden."));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending existing coupon: {ex.Message}");
            return Result.Failure(new Error("SendFailed", "Fehler beim Senden des Coupons."));
        }
    }
} 