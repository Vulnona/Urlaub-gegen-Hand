using UGHApi.Entities;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction> CreateTransactionRecord(Transaction transaction);
    Task<Transaction> GetTransactionByTransactionId(string transactionId);
    Task<Transaction> GetPaymentIntentIdById(int id);
    Task<PaginatedList<TransactionDto>> GetUserTransactionsByUserId(
        Guid userId,
        int pageNumber,
        int pageSize
    );
}
