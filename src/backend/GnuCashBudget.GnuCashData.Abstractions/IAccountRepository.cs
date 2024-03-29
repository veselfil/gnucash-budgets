using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.Abstractions;

public interface IAccountRepository
{
    Task<Account?> Find(string accountId);
    Task<ImmutableList<Account>> GetAllAccounts();
    Task<ImmutableList<Account>> GetAccountsByType(AccountType type);
    Task<(Account?,Commodity?)> GetParentAccountByType(AccountType type, bool includeRootAccount);
    Task<Account> CreateAccount(AccountType type, string parentId, string commodityId, int commodityFraction);
}