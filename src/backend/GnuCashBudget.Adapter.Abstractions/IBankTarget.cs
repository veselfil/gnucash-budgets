using GnuCashBudget.Adapter.Abstractions.Models;

namespace GnuCashBudget.Adapter.Abstractions;

// The Target defines the domain-specific interface used by the client code.
public interface IBankTarget
{
    Task<AdapterResponse> GetExpensesHistoryAsync(string? continuationToken);
}