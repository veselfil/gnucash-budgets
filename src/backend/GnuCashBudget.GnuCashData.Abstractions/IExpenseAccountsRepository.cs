using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.Abstractions;

public interface IExpenseAccountsRepository
{
    Task<ExpenseAccount> Find(string accountGuid);
    Task<ImmutableList<ExpenseAccount>> GetAllExpenseAccounts();
}