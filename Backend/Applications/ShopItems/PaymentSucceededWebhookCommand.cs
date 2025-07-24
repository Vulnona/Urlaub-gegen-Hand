using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.ShopItems;

public class PaymentSucceededWebhookCommand : IRequest<Result>
{
    public string JsonPayload { get; set; }
    public string StripeSignature { get; set; }
}
