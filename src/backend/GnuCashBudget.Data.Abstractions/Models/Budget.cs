namespace GnuCashBudget.Data.EntityFramework.Models;

public record Budget(int Id, int BudgetedAccount, DateTime ValidFrom, DateTime ValidTo, decimal Amount);