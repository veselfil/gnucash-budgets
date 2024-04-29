using GnuCashBudget.Adapter.Abstractions.Models;

namespace GnuCashBudget.Adapter.Example.Services;

public interface IAdapterService
{
    Task<AdapterResponse> GetExpensesHistory(string? continuationToken);
}