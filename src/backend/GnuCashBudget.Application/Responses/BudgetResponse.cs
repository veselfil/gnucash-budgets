namespace GnuCashBudget.Application.Responses;

public record BudgetResponse(
    int BudgetId,
    int BudgetedAccountId, 
    decimal Amount,
    DateTime StartDate, 
    DateTime EndDate);
