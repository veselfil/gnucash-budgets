using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.Generator.Interfaces;

public interface IGeneratorService
{
    Task<Account> GetOrCreateAccountAsync(AccountType accountType, string? parentId, string? commodityId,
        int commodityFraction);

    Task<ImmutableList<Account>> CreateChildAccounts(int accountsCount, AccountType accountType, string parentId,
        string commodityId, int commodityFraction);

    Task CreateTransaction(Account accountFrom, Account accountTo, Commodity commodity, int amount, string description);
    Stack<int> GenerateStackOfAllExpenses(int incomeAmount, int maxPriceOfOneExpense, int percentageToExpense);
}