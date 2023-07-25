namespace GnuCashBudget.GnuCashData.Abstractions.Models;

public record AccountTransactionView(string AccountId, string AccountName, decimal TransactionAmount, DateTime Date);
