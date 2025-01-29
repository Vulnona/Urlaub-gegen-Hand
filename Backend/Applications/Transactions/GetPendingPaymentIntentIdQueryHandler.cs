using MediatR;
using UGH.Domain.Core;
using UGHApi.Interfaces;
using UGHApi.Services.Stripe;

namespace UGHApi.Applications.Transactions;

public class GetPendingPaymentIntentIdQueryHandler
    : IRequestHandler<GetPendingPaymentIntentIdQuery, Result>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IStripeService _stripeService;

    public GetPendingPaymentIntentIdQueryHandler(
        ITransactionRepository transactionRepository,
        IStripeService stripeService
    )
    {
        _transactionRepository = transactionRepository;
        _stripeService = stripeService;
    }

    public async Task<Result> Handle(
        GetPendingPaymentIntentIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await _transactionRepository.GetPaymentIntentIdById(
            request.TransactionId
        );

        if (transaction == null)
        {
            return Result.Failure(new Error("TransactionNotFound", "Transaction not found."));
        }

        if (transaction.UserId != request.UserId)
        {
            return Result.Failure(Errors.General.InvalidOperation("User Unauthorized."));
        }

        try
        {
            var clientSecret = await _stripeService.GetClientSecretAsync(transaction.TransactionId);

            return Result.Success(clientSecret);
        }
        catch (Exception ex)
        {
            return Result.Failure(
                new Error("StripeApiError", $"Error fetching client secret: {ex.Message}")
            );
        }
    }
}
