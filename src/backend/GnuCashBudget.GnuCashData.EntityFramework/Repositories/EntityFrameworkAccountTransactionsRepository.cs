using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.GnuCashData.EntityFramework;

public class EntityFrameworkAccountTransactionsRepository : IAccountTransactionsRepository
{
    private readonly GnuCashContext _context;

    public EntityFrameworkAccountTransactionsRepository(GnuCashContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AccountTransactionView>> GetTransactionsForAccountInDateRange(
        Account account,
        DateTime from,
        DateTime to)
    {
        string accountId = account.Id;
        var transactions = await _context.Splits
            .Where(x => x.AccountId == accountId)
            .Join(_context.Transactions,
                split => split.TxId,
                transaction => transaction.Id,
                (split, transaction) => new { split, transaction })
            .Join(_context.Accounts,
                x => x.split.AccountId,
                a => a.Id,
                (x, a) => new { x.split, x.transaction, account })
            .Where(x => x.transaction.PostDate >= from & x.transaction.PostDate <= to)
            .Select(x =>
                new AccountTransactionViewEntity(x.account.Id, x.account.Name, x.split.QuantityNum, x.split.QuantityDenom,
                    x.transaction.PostDate))
            .ToListAsync();

        return transactions
            .Select(t => t.ToDomainView())
            .ToList();
    }

    public async Task<decimal> GetSumOfTransactions(Account account, DateTime from, DateTime to)
    {
        string accountId = account.Id;
        var transactions = await _context.Splits
            .Where(x => x.AccountId == accountId)
            .Join(_context.Transactions,
                split => split.TxId,
                transaction => transaction.Id,
                (split, transaction) => new { split, transaction })
            .Join(_context.Accounts,
                x => x.split.AccountId,
                a => a.Id,
                (x, a) => new { x.split, x.transaction, account })
            .Where(x => x.transaction.PostDate >= from & x.transaction.PostDate <= to)
            .Select(x => new { x.split.QuantityDenom, x.split.QuantityNum })
            .ToListAsync();
        
        return transactions.Sum(x => (decimal)x.QuantityNum / x.QuantityDenom);

    }

    private record AccountTransactionViewEntity(string AccountId, string AccountName, int ValueNum, int ValueDenom,
        DateTime PostDate)
    {
        public AccountTransactionView ToDomainView() =>
            new AccountTransactionView(AccountId, AccountName, (decimal)ValueNum / ValueDenom, PostDate);
    }
}