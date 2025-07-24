using MediatR;
using UGHApi.Models;

namespace UGHApi.Applications.Coupons;

public class CreatePaymentIntentCommand : IRequest<CreatePaymentIntentResponse>
{
    public Guid UserId { get; set; }
    public int ShopItemId { get; set; }
}
