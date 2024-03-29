using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.Generator.Configuration;
using GnuCashBudget.GnuCashData.Generator.Interfaces;
using Microsoft.Extensions.Options;

namespace GnuCashBudget.GnuCashData.Generator.Services;

public class GeneratorService(
    IOptions<GeneratorOptions> generatorOptions,
    IAccountRepository accountRepository) : IGeneratorService
{
    private readonly GeneratorOptions _generatorOptions = generatorOptions.Value;

    public async Task GenerateExpenses(CancellationToken cancellationToken) // TODO finish it
    {
        // Základ je mít ROOT ACCOUNT!
        // Je potřeba vytvořit prvně Income účet, kam půjde "výplata"
        // Potom je potřeba vytvořit Bank účet, na který bude napojený Income, např. "ČSOB" účet
        // A nakonec je nutné vytvořit Expense účet/účty, které budou čerpat z Bank účtu, např. "Revolut"
        // Na generování jmen vytvořit enum, ze kterého se random vybere?

        // V rámci generátoru budeme vytvářet 1 Income účet, 1 Bank účet a pak N Expense účtů dle options
        // Income půjde z options
        // Bank účet automaticky nabijeme tím co bude v Income (začínáme s čistým účtem -> žádné dluhy ani spoření)
        // Expenses zadáme v options počet účtů a poté procento, kolik Income se má vyčerpat

        // Vždy budeme chtít účty vytvořit! Nikdy nechceme používat stávající účty. Jediné co budeme kontrolovat je to,
        // zda neexistuje hlavní Income/Bank/Expense account. Pokud ano, budeme pod ním už jen vytvářet Child/ren
        
        var (rootAccount, rootCommodity) = await accountRepository.GetParentAccountByType(AccountType.Root, true);
        if (rootAccount is null || rootCommodity is null)
        {
            throw new Exception("Cannot proceed further"); // TODO edit this error
        }
        Console.WriteLine($"Root account was found, name: {rootAccount.Name}, id: {rootAccount.Id}");
        
        var incomeAccount = await GetOrCreateAccountAsync(AccountType.Income, rootAccount.Id, rootCommodity.Id, rootCommodity.Fraction);
        Console.WriteLine($"Main INCOME account created/fetched, name: {incomeAccount.Name}, id: {incomeAccount.Id}");
        
        var bankAccount = await GetOrCreateAccountAsync(AccountType.Bank, rootAccount.Id, rootCommodity.Id, rootCommodity.Fraction);
        Console.WriteLine($"Main BANK account created/fetched, name: {bankAccount.Name}, id: {bankAccount.Id}");
        
        var expenseAccount = await GetOrCreateAccountAsync(AccountType.Expense, rootAccount.Id, rootCommodity.Id, rootCommodity.Fraction);
        Console.WriteLine($"Main EXPENSE account created/fetched, name: {expenseAccount.Name}, id: {expenseAccount.Id}");
        
    }

    private async Task<Account> GetOrCreateAccountAsync(AccountType accountType, string? parentId, string? commodityId, int? commodityFraction)
    {
        var (account, _) = await accountRepository.GetParentAccountByType(accountType, parentId is null);

        if (account is not null)
        {
            return account;
        }

        if (parentId is null || commodityId is null)
        {
            throw new Exception("Cannot create an account!");
        }
        
        return await accountRepository.CreateAccount(accountType, parentId, commodityId, commodityFraction ?? 100);
    }
}