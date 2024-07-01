namespace GnuCashBudget.Adapter.Abstractions.Models;

public class AdapterResponse
{
    public string? ContinuationToken { get; set; }
    public IEnumerable<Expense> Transactions { get; set; }
}