using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Transactions;

public class GetPendingPaymentIntentIdQuery : IRequest<Result>
{
    public Guid UserId { get; set; }
    public int TransactionId { get; set; }
}
