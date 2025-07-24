using Mapster;
using Microsoft.EntityFrameworkCore;
using UGHApi.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;
using UGHApi.DATA;

public class TransactionRepository : ITransactionRepository
{
    private readonly Ugh_Context _context;

    public TransactionRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<Transaction> CreateTransactionRecord(Transaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        transaction.TransactionDate = DateTime.UtcNow;
        _context.Set<Transaction>().Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> GetPaymentIntentIdById(int id)
    {
        return await _context.transaction.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Transaction> GetTransactionByTransactionId(string transactionId)
    {
        return await _context
            .transaction.Include(t => t.User)
            .Include(t => t.ShopItem)
            .FirstOrDefaultAsync(x => x.TransactionId == transactionId);
    }

    public async Task<PaginatedList<TransactionDto>> GetUserTransactionsByUserId(
        Guid userId,
        int pageNumber,
        int pageSize
    )
    {
        IQueryable<Transaction> query = _context
            .transaction.Include(t => t.ShopItem)
            .Include(t => t.Coupon)
            .ThenInclude(c => c.Redemption)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TransactionDate);

        int totalCount = await query.CountAsync();

        List<Transaction> transactions = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        List<TransactionDto> transactionDtos = transactions.Adapt<List<TransactionDto>>();

        return PaginatedList<TransactionDto>.Create(
            transactionDtos,
            totalCount,
            pageNumber,
            pageSize
        );
    }
}
