using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.Abstractions;

public interface IAccountTransactionsRepository
{
    Task<IEnumerable<AccountTransactionView>> GetTransactionsForAccountInDateRange(Account account, DateTime from, DateTime to);
}