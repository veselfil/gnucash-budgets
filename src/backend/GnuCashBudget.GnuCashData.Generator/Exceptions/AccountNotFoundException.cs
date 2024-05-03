namespace GnuCashBudget.GnuCashData.Generator.Exceptions;

public class AccountNotFoundException(string accountType) : Exception($"Account with type {accountType} was not found");