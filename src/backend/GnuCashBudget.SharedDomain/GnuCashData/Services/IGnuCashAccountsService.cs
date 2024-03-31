using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.EntityFramework.Services;

public interface IGnuCashAccountsService
{
    Task<Account?> Find(string accountId);
    Task<ImmutableList<Account>> GetAllAccounts();
    Task<ImmutableList<Account>> GetAccountsByType(AccountType type);

    Task<IEnumerable<AccountTransactionView>> GetTransactionsForAccountInDateRange(
        string accountId,
        DateTime from,
        DateTime to);
}