using MediatR;
using UGH.Domain.Core;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Transactions;

public class GetUserTransactionQuery : IRequest<Result<PaginatedList<TransactionDto>>>
{
    public Guid UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetUserTransactionQuery(Guid userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
