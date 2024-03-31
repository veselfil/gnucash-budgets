using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using GnuCashBudget.GnuCashData.EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.GnuCashData.EntityFramework.Repositories;

public class EntityFrameworkAccountTransactionsRepository(GnuCashContext context) : IAccountTransactionsRepository
{
    public async Task<IEnumerable<AccountTransactionView>> GetTransactionsForAccountInDateRange(
        string accountId,
        DateTime from,
        DateTime to)
    {
        var transactions = await context.Splits
            .Where(x => x.AccountId == accountId)
            .Join(context.Transactions,
                split => split.TxId,
                transaction => transaction.Id,
                (split, transaction) => new { split, transaction })
            .Join(context.Accounts,
                x => x.split.AccountId,
                a => a.Id,
                (x, a) => new { x.split, x.transaction, account = a })
            .Where(x => x.transaction.PostDate >= from & x.transaction.PostDate <= to)
            .Select(x =>
                new AccountTransactionViewEntity(x.account.Id, x.account.Name, x.split.QuantityNum, x.split.QuantityDenom,
                    x.transaction.PostDate))
            .ToListAsync();

        return transactions
            .Select(t => t.ToDomainView())
            .ToList();
    }

    public async Task<decimal> GetSumOfTransactions(string accountId, DateTime from, DateTime to)
    {
        var transactions = await context.Splits
            .Where(x => x.AccountId == accountId)
            .Join(context.Transactions,
                split => split.TxId,
                transaction => transaction.Id,
                (split, transaction) => new { split, transaction })
            .Join(context.Accounts,
                x => x.split.AccountId,
                a => a.Id,
                (x, a) => new { x.split, x.transaction })
            .Where(x => x.transaction.PostDate >= from & x.transaction.PostDate <= to)
            .Select(x => new { x.split.QuantityDenom, x.split.QuantityNum })
            .ToListAsync();
        
        return transactions.Sum(x => (decimal)x.QuantityNum / x.QuantityDenom);

    }

    public async Task WriteTransactionAsync(
        Account accountFrom, Account accountTo,
        Commodity commodity, Transaction transaction,
        CancellationToken cancellationToken)
    {
        var transactionEntity = MapTransactionEntity(commodity, transaction);

        var splitEntityFrom = MapSplitEntity(transactionEntity.Id, accountFrom, transaction, SplitType.Debit);
        var splitEntityTo = MapSplitEntity(transactionEntity.Id, accountTo, transaction, SplitType.Credit);
        
        await context.Transactions.AddAsync(transactionEntity, cancellationToken);
        await context.Splits.AddAsync(splitEntityFrom, cancellationToken);
        await context.Splits.AddAsync(splitEntityTo, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private record AccountTransactionViewEntity(string AccountId, string AccountName, int ValueNum, int ValueDenom,
        DateTime PostDate)
    {
        public AccountTransactionView ToDomainView() =>
            new AccountTransactionView(AccountId, AccountName, (decimal)ValueNum / ValueDenom, PostDate);
    }

    private TransactionEntity MapTransactionEntity(Commodity commodity, Transaction transaction)
    {
        return new TransactionEntity
        {
            Id = SimpleHelper.GenerateGuid(),
            CurrencyId = commodity.Id,
            Description = transaction.Description,
            EntryDate = transaction.EntryDate,
            Num = string.Empty,
            PostDate = transaction.PostDate,
        };
    }

    private SplitEntity MapSplitEntity(string transactionId, Account account, Transaction transaction, SplitType splitType)
    {
        return new SplitEntity
        {
            Id = SimpleHelper.GenerateGuid(),
            TxId = transactionId,
            AccountId = account.Id,
            Memo = string.Empty,
            Action = string.Empty,
            ReconcileState = false,
            ReconcileDate = DateTime.UnixEpoch.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"),
            ValueNum = (int)transaction.ValueNum * splitType.ConvertTypeToInt(),
            ValueDenom = (int)transaction.ValueDenom,
            QuantityNum = (int)transaction.QuantityNum * splitType.ConvertTypeToInt(),
            QuantityDenom = (int)transaction.QuantityDenom,
            LotId = null,
        };
    }
}