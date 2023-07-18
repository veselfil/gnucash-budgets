using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.Abstractions;

public interface IAccountRepository
{
    Task<ImmutableList<Account>> GetAllAccounts();
    Task<ImmutableList<Account>> GetAccountsByType(AccountType type);
}