using GnuCashBudget.Adapter.Abstractions;
using GnuCashBudget.Adapter.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Adapter.Example.Controllers;

[ApiController]
[Route("expenses")]
public class ExampleController(IBankTarget adapter) : Controller
{
    [HttpGet("history")]
    public async Task<AdapterResponse> GetExpensesHistory(
        [FromQuery] string? continuationToken)
    {
        return await adapter.GetExpensesHistoryAsync(continuationToken);
    }
}