using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.Abstractions;

public interface IAccountTransactionsRepository
{
    Task<IEnumerable<AccountTransactionView>> GetTransactionsForAccountInDateRange(string accountId, DateTime from, DateTime to);
    Task<decimal> GetSumOfTransactions(string accountId, DateTime from, DateTime to);
    Task WriteTransactionAsync(Account accountFrom, Account accountTo, Commodity commodity, Transaction transaction);
}