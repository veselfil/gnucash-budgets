using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.Generator.Exceptions;
using GnuCashBudget.GnuCashData.Generator.Interfaces;
using Microsoft.Extensions.Logging;

namespace GnuCashBudget.GnuCashData.Generator.Services;

public class GeneratorService(
    ILogger<GeneratorService> logger,
    IAccountRepository accountRepository,
    IAccountTransactionsRepository transactionsRepository) : IGeneratorService
{
    private static readonly Random Random = new();

    public async Task<Account> GetOrCreateAccountAsync(AccountType accountType, string? parentId, string? commodityId,
        int commodityFraction)
    {
        var (account, _) = await accountRepository.GetParentAccountByType(accountType, parentId is null);
        if (account is not null)
        {
            return account;
        }

        if (parentId is null || commodityId is null)
        {
            throw new AccountCreationException($"Cannot create an account! Either ParentId({parentId}) or CommodityId({commodityId}) are NULL");
        }

        return await accountRepository.CreateAccount(accountType, parentId, commodityId, commodityFraction);
    }

    public Task<ImmutableList<Account>> CreateChildAccounts(int accountsCount, AccountType accountType, string parentId,
        string commodityId, int commodityFraction)
    {
        return Task.FromResult(
            Enumerable.Range(1, accountsCount)
                .Select(async x =>
                    await accountRepository.CreateAccount(accountType, parentId, commodityId, commodityFraction))
                .Select(t => t.Result)
                .ToImmutableList()
        );
    }

    public async Task CreateTransaction(Account accountFrom, Account accountTo, Commodity commodity, int amount, string description)
    {
        // var description = typeof(ExpensesDescriptionType).PickAtRandom<ExpensesDescriptionType>().DisplayName(); // TODO this could be done better

        var transaction = MapTransaction(
            description,
            (uint)amount,
            (uint)commodity.Fraction
        );

        await transactionsRepository.WriteTransactionAsync(accountFrom, accountTo, commodity, transaction);
    }

    private Transaction MapTransaction(string description, uint amount, uint fraction)
    {
        return new Transaction(
            Description: description,
            PostDate: DateTime.Now,
            EntryDate: DateTime.Now,
            ValueNum: amount * fraction,
            ValueDenom: fraction,
            QuantityNum: amount * fraction,
            QuantityDenom: fraction
        );
    }

    public Stack<int> GenerateStackOfAllExpenses(int incomeAmount, int maxPriceOfOneExpense, int percentageToExpense)
    {
        var amountLeftForExpense = incomeAmount * percentageToExpense / 100;
        var result = new Stack<int>();

        // If the user inputs maxPriceOfOneExpense which is higher than amountLeftForExpense we will just change
        // maxPriceOfOneExpense and let calculations go on
        if (maxPriceOfOneExpense >= amountLeftForExpense)
        {
            logger.LogInformation(
                "User inputted maxPriceOfOneExpense({OneExpense}) which is higher than amountLeftForExpense({AmountLeftForExpense}). Changing maxPriceOfOneExpense to {Amount}",
                maxPriceOfOneExpense, amountLeftForExpense, amountLeftForExpense);
            maxPriceOfOneExpense = amountLeftForExpense;
        }

        while (amountLeftForExpense > 0)
        {
            var expense = Random.Next(1, maxPriceOfOneExpense);
            if (expense > amountLeftForExpense)
            {
                result.Push(amountLeftForExpense);
                break;
            }

            result.Push(expense);
            amountLeftForExpense -= expense;
        }

        return result;
    }
}