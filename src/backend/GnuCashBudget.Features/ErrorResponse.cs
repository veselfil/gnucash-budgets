namespace GnuCashBudget.Feature.BudgetedAccounts;

public record ErrorResponseBase(ErrorType ErrorType)
{
    public bool Success => ErrorType == ErrorType.None; 
}
