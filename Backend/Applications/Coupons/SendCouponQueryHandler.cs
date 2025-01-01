using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;
using UGHApi.Applications.Coupons;
using UGHApi.Interfaces;
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
        var coupon = await _couponRepository.IsCouponExists(requestCode);
        if (!coupon)
        {
            return Result.Failure(Errors.General.NotFound("Coupon", requestCode));
        }

        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(Errors.General.NotFound("User", request.UserId));

        Task.Run(async () =>
        {
            try
            {
                var htmlTemplate = _htmlTemplateService.GetCouponReceivedDetails(
                    requestCode,
                    $"{user.FirstName} {user.LastName}".Trim()
                );

                await _emailService.SendEmailAsync(
                    user.Email_Address,
                    htmlTemplate.Subject,
                    htmlTemplate.BodyHtml
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email to {user.Email_Address}: {ex.Message}");
            }
        });

        return Result.Success("Coupon sent successfully.");
    }
}
