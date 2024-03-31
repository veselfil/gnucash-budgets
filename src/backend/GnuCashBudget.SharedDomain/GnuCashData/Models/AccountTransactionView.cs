namespace GnuCashBudget.GnuCashData.Abstractions.Models;

public record AccountTransactionView(
    string AccountId,
    string AccountName,
    string Description,
    decimal TransactionAmount,
    DateTime Date);
