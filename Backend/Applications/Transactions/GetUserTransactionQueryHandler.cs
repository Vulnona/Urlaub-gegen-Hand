using MediatR;
using UGH.Domain.Core;
using UGHApi.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Transactions;

public class GetUserTransactionQueryHandler
    : IRequestHandler<GetUserTransactionQuery, Result<PaginatedList<TransactionDto>>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<GetUserTransactionQueryHandler> _logger;

    public GetUserTransactionQueryHandler(
        ITransactionRepository transactionRepository,
        ILogger<GetUserTransactionQueryHandler> logger
    )
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<TransactionDto>>> Handle(
        GetUserTransactionQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedTransactions = await _transactionRepository.GetUserTransactionsByUserId(
                request.UserId,
                request.PageNumber,
                request.PageSize
            );

            return Result<PaginatedList<TransactionDto>>.Success(paginatedTransactions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<PaginatedList<TransactionDto>>(
                Errors.General.InvalidOperation("Something went wrong while fetching transactions")
            );
        }
    }
}
