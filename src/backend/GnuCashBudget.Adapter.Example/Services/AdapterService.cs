using GnuCashBudget.Adapter.Abstractions.Models;
using GnuCashBudget.Adapter.Example.Configuration;
using Microsoft.Extensions.Options;

namespace GnuCashBudget.Adapter.Example.Services;

public class AdapterService(IOptions<ExampleOptions> exampleOptions) : IAdapterService
{
    public async Task<AdapterResponse> GetExpensesHistory(string? continuationToken)
    {
        // Normally we would use DI and register Adaptee and Adapter
        // I am not doing it right now, so we can see the pattern clearly
        var adaptee = new Adaptee(exampleOptions);
        var adapter = new Adapter(adaptee);

        return await adapter.GetExpensesHistoryAsync(continuationToken);
    }
}