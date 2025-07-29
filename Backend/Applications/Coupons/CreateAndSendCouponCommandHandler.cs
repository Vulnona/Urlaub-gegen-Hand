using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Extensions;
using UGHApi.Services.HtmlTemplate;
using UGH.Infrastructure.Services;

namespace UGHApi.Applications.Coupons;

public class CreateAndSendCouponCommandHandler : IRequestHandler<CreateAndSendCouponCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly HtmlTemplateService _htmlTemplateService;
    private readonly EmailService _emailService;
    private readonly ILogger<CreateAndSendCouponCommandHandler> _logger;

    public CreateAndSendCouponCommandHandler(
        IUserRepository userRepository,
        ICouponRepository couponRepository,
        IMembershipRepository membershipRepository,
        HtmlTemplateService htmlTemplateService,
        EmailService emailService,
        ILogger<CreateAndSendCouponCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _couponRepository = couponRepository;
        _membershipRepository = membershipRepository;
        _htmlTemplateService = htmlTemplateService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateAndSendCouponCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Find user by email
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure(new Error("UserNotFound", "Benutzer mit dieser E-Mail-Adresse wurde nicht gefunden."));
            }

            // Check if user is verified
            if (!user.IsEmailVerified)
            {
                return Result.Failure(new Error("UserNotVerified", "Benutzer ist nicht verifiziert."));
            }

            // Get membership
            var membership = await _membershipRepository.GetMembershipByIdAsync(request.MembershipId);
            if (membership == null)
            {
                return Result.Failure(new Error("MembershipNotFound", "Mitgliedschaft nicht gefunden."));
            }

            // Create coupon
            var newCoupon = new Coupon
            {
                Code = Ulid.NewUlid().ToString(),
                Name = request.Name ?? $"Coupon für {user.FirstName} {user.LastName}",
                Description = string.Empty,
                CreatedBy = user.User_Id, // Admin creates it for the user
                MembershipId = membership.MembershipID,
                Duration = membership.DurationDays.ToCouponDuration(),
            };

            var coupon = await _couponRepository.AddCoupon(newCoupon);

            // Send email
            var htmlTemplate = _htmlTemplateService.GetCouponReceivedDetails(
                coupon.Code,
                $"{user.FirstName} {user.LastName}".Trim(),
                membership.Name,
                (int)Math.Round(membership.DurationDays / 30.0)
            );

            var emailSent = await _emailService.SendEmailAsync(
                user.Email_Address,
                htmlTemplate.Subject,
                htmlTemplate.BodyHtml
            );

            if (emailSent)
            {
                // Update coupon with email tracking information
                coupon.IsEmailSent = true;
                coupon.EmailSentDate = DateTime.Now;
                coupon.EmailSentTo = user.Email_Address;
                await _couponRepository.UpdateCouponAsync(coupon);

                _logger.LogInformation($"Coupon {coupon.Code} created and sent to {user.Email_Address}");
                return Result.Success("Coupon erstellt und E-Mail erfolgreich gesendet.");
            }
            else
            {
                _logger.LogError($"Failed to send coupon {coupon.Code} to {user.Email_Address}");
                return Result.Failure(new Error("EmailSendFailed", "E-Mail konnte nicht gesendet werden."));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating and sending coupon: {ex.Message}");
            return Result.Failure(new Error("CreationFailed", "Fehler beim Erstellen und Senden des Coupons."));
        }
    }
} 