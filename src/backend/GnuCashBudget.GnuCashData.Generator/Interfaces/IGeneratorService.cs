using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.Generator.Interfaces;

public interface IGeneratorService
{
    Task<Account> GetOrCreateAccountAsync(AccountType accountType, string? parentId, string? commodityId,
        int commodityFraction, CancellationToken cancellationToken = default);

    Task<ImmutableList<Account>> CreateChildAccounts(int accountsCount, AccountType accountType, string parentId,
        string commodityId, int commodityFraction, CancellationToken cancellationToken = default);

    Task CreateTransaction(Account accountFrom, Account accountTo, Commodity commodity, int amount, string description, CancellationToken cancellationToken = default);
    Stack<int> GenerateStackOfAllExpenses(int incomeAmount, int maxPriceOfOneExpense, int percentageToExpense);
}