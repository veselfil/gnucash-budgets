using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.Generator.Configuration;
using GnuCashBudget.GnuCashData.Generator.Enums;
using GnuCashBudget.GnuCashData.Generator.Exceptions;
using GnuCashBudget.GnuCashData.Generator.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GnuCashBudget.GnuCashData.Generator.Services;

public sealed class WorkerService(
    ILogger<WorkerService> logger,
    IOptions<GeneratorOptions> generatorOptions,
    IGeneratorService generatorService,
    IAccountRepository accountRepository) : IScopedProcessingService
{
    private readonly GeneratorOptions _generatorOptions = generatorOptions.Value;
    private static readonly Random Random = new();

    public async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        var (rootAccount, rootCommodity) = await GetRootAccount(cancellationToken);

        var (_, incomeChildren) = await GetAccounts(AccountType.Income, rootAccount.Id, rootCommodity.Id,
            rootCommodity.Fraction, 1, cancellationToken);
        var incomeChild =
            incomeChildren[0]; // We can use index 0 here because we are generating only 1 account

        var (bankAccount, bankChildren) = await GetAccounts(AccountType.Bank, rootAccount.Id, rootCommodity.Id,
            rootCommodity.Fraction, 1, cancellationToken);
        var bankChild =
            bankChildren[0]; // We can use index 0 here because we are generating only 1 account

        var (_, expenseChildren) = await GetAccounts(AccountType.Expense, rootAccount.Id, rootCommodity.Id,
            rootCommodity.Fraction, _generatorOptions.ExpenseAccountCount, cancellationToken);

        await generatorService.CreateTransaction(incomeChild, bankChild, rootCommodity, _generatorOptions.IncomeAmount,
            GetDescription(AccountType.Income), cancellationToken);

        var expenses = generatorService.GenerateStackOfAllExpenses(_generatorOptions.IncomeAmount,
            _generatorOptions.MaxPriceOfOneExpense, _generatorOptions.PercentageToExpense);
        await WriteExpensesToAccount(expenses, expenseChildren, bankAccount, rootCommodity, cancellationToken);

        logger.LogInformation("Generating of expenses done! Job is finished");
    }

    private async Task<(Account, Commodity)> GetRootAccount(CancellationToken cancellationToken)
    {
        var (rootAccount, rootCommodity) =
            await accountRepository.GetParentAccountByType(AccountType.Root, true, cancellationToken);
        if (rootAccount is null)
        {
            throw new AccountNotFoundException(AccountType.Root.ToString());
        }

        if (rootCommodity is null)
        {
            throw new CommodityNotFoundException(rootAccount.Id);
        }

        Console.WriteLine($"Root account was found, name: {rootAccount.Name}, id: {rootAccount.Id}");

        return (rootAccount, rootCommodity);
    }

    private async Task<(Account, ImmutableList<Account>)> GetAccounts(AccountType type, string parentId,
        string commodityId, int commodityFraction, int numberOfChildren, CancellationToken cancellationToken)
    {
        var parentAccount =
            await generatorService.GetOrCreateAccountAsync(type, parentId, commodityId, commodityFraction,
                cancellationToken);

        var childAccounts = await generatorService.CreateChildAccounts(numberOfChildren,
            type,
            parentAccount.Id,
            commodityId,
            commodityFraction, cancellationToken);

        logger.LogInformation("Main {Type} account created/fetched, name: {ParentAccountName}, id: {ParentAccountId}",
            type.ToString().ToUpper(), parentAccount.Name, parentAccount.Id);

        childAccounts.ForEach(x =>
            logger.LogInformation("Child {Type} account created, name: {ObjName}, id: {ObjId}",
                x.AccountType.ToString().ToUpper(), x.Name, x.Id));

        return (parentAccount, childAccounts);
    }

    private async Task WriteExpensesToAccount(Stack<int> expenses, ImmutableList<Account> expenseChildren,
        Account accountFrom, Commodity commodity, CancellationToken cancellationToken)
    {
        while (expenses.Count != 0)
        {
            var account = expenseChildren[Random.Next(0, expenseChildren.Count)];
            var expenseAmount = expenses.Pop();

            await generatorService.CreateTransaction(accountFrom, account, commodity, expenseAmount,
                GetDescription(AccountType.Expense), cancellationToken);
            logger.LogInformation(
                "Wrote transaction from {AccountFromAccountType} account to {AccountAccountType} account with {CommodityMnemonic}{ExpenseAmount}",
                accountFrom.AccountType, account.AccountType, commodity.Mnemonic, expenseAmount);
        }
    }

    private string GetDescription(AccountType accountType)
    {
        return accountType switch
        {
            AccountType.Income => EnumExtensions.PickAtRandom<IncomeDescriptionType>().DisplayName(),
            AccountType.Bank or AccountType.Expense => EnumExtensions.PickAtRandom<ExpensesDescriptionType>()
                .DisplayName(),
            _ => "Default Transaction Description"
        };
    }
}