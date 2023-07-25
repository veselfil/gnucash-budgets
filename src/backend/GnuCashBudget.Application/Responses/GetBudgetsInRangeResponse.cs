namespace GnuCashBudget.Application.Responses;

public class GetBudgetsInRangeResponse
{
    public IEnumerable<BudgetResponse> Budgets { get; set; }
}