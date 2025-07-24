using System.Text.Json;
using MediatR;
using Stripe;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;
using UGHApi.Extensions;
using UGH.Domain.Interfaces;
using UGHApi.Services.HtmlTemplate;
using static UGH.Domain.Core.UGH_Enums;

namespace UGHApi.Applications.ShopItems;

#pragma warning disable CS4014

public class PaymentSucceededWebhookCommandHandler
    : IRequestHandler<PaymentSucceededWebhookCommand, Result>
{
    private readonly ILogger<PaymentSucceededWebhookCommandHandler> _logger;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly string _stripeWebhookSecret;
    private readonly HtmlTemplateService _htmlTemplateService;
    private readonly EmailService _emailService;

    public PaymentSucceededWebhookCommandHandler(
        ILogger<PaymentSucceededWebhookCommandHandler> logger,
        ITransactionRepository transactionRepository,
        IConfiguration configuration,
        IMembershipRepository membershipRepository,
        ICouponRepository couponRepository,
        HtmlTemplateService htmlTemplateService,
        EmailService emailService
    )
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
        _couponRepository = couponRepository;
        _stripeWebhookSecret = configuration["Stripe:PaymentSuccessWebhookId"];
        _htmlTemplateService = htmlTemplateService;
        _membershipRepository = membershipRepository;
        _emailService = emailService;
    }

    public async Task<Result> Handle(
        PaymentSucceededWebhookCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (
                string.IsNullOrWhiteSpace(request.JsonPayload)
                || string.IsNullOrWhiteSpace(request.StripeSignature)
            )
            {
                _logger.LogWarning("Invalid webhook request: Missing payload or signature.");
                return Result.Failure(Errors.General.InvalidOperation("Invalid webhook request."));
            }

            _logger.LogInformation($"Received Stripe Webhook: {request.JsonPayload}");

            var stripeEvent = EventUtility.ConstructEvent(
                request.JsonPayload,
                request.StripeSignature,
                _stripeWebhookSecret,
                throwOnApiVersionMismatch: false
            );

            if (stripeEvent.Type != "payment_intent.succeeded")
            {
                return Result.Failure(Errors.General.InvalidOperation("Unsupported webhook event type."));
            }

            var paymentIntent = stripeEvent.Data.Object as Stripe.PaymentIntent;
            var paymentMethodId = paymentIntent.PaymentMethodId;

            var service = new PaymentMethodService();
            var paymentMethod = await service.GetAsync(paymentMethodId);
            var paymentMethodType = paymentMethod.Type;

            var paymentIntentId = ExtractPaymentIntentId(request.JsonPayload);

            var transaction = await _transactionRepository.GetTransactionByTransactionId(paymentIntentId);

            if (transaction == null)
            {
                _logger.LogInformation($"Transaction not found with Payment intent Id: {paymentIntentId}");
                return Result.Failure(Errors.General.NotFound("Transaction", paymentIntentId));
            }

            var membership = await _membershipRepository.GetMembershipByDurationDaysAsync(
                transaction.ShopItem.Duration.ToDays()
            );

            if (membership == null)
            {
                _logger.LogInformation(
                    $"Membership with duration of {transaction.ShopItem.Duration.ToDays()} days not found."
                );
                return Result.Failure(Errors.General.InvalidOperation("Shop Item does not belong to any membership"));
            }

            var newCoupon = new UGH.Domain.Entities.Coupon
            {
                Code = Ulid.NewUlid().ToString(),
                Name = transaction.ShopItem.Name,
                Description = string.Empty,
                CreatedBy = transaction.UserId,
                MembershipId = membership.MembershipID,
                Duration = membership.DurationDays.ToCouponDuration(),
            };

            var coupon = await _couponRepository.AddCoupon(newCoupon);

            transaction.AssignCouponId(coupon.Id);
            transaction.UpdateTransactionStatus(TransactionStatus.Complete);
            transaction.SetPaymentMethod(paymentMethodType);
            await _couponRepository.SaveChangesAsync();

            // Send Mail Here
            Task.Run(async () =>
            {
                try
                {
                    var htmlTemplate = _htmlTemplateService.GetCouponPurchasedDetails(
                        coupon.Code,
                        $"{transaction.User.FirstName} {transaction.User.LastName}".Trim()
                    );

                    await _emailService.SendEmailAsync(
                        coupon.CreatedByUser.Email_Address,
                        htmlTemplate.Subject,
                        htmlTemplate.BodyHtml
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        $"Error sending email to {coupon.CreatedByUser.Email_Address}: {ex.Message}"
                    );
                }
            });

            return Result.Success("Data updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Stripe Webhook: {ex.Message}");
            return Result.Failure(Errors.General.InvalidOperation(ex.Message));
        }
    }


    private string ExtractPaymentIntentId(string jsonPayload)
    {
        var document = JsonDocument.Parse(jsonPayload);
        return document
            .RootElement.GetProperty("data")
            .GetProperty("object")
            .GetProperty("id")
            .GetString()
            ?.Trim();
    }
}
