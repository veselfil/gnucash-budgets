namespace GnuCashBudget.Adapter.Abstractions.Models;

public class AdapterResponse
{
    public string? ContinuationToken { get; set; }
    public IEnumerable<Expense> Expenses { get; set; }
}