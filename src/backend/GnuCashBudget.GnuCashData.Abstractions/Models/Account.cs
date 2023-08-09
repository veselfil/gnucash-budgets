using System.Collections.Immutable;

namespace GnuCashBudget.GnuCashData.Abstractions.Models;

public record Account(
    string Id,
    string Name,
    string FullName,
    string Commodity,
    AccountType AccountType,
    ImmutableList<Account> ChildAccounts);
    
public record ExpenseAccount(string Id,
    string Name,
    string FullName,
    string Commodity,
    AccountType AccountType,
    ImmutableList<ExpenseAccount> ChildAccounts,
    string CurrencyCode);