using GnuCashBudget.Adapter.Abstractions.Models;
using GnuCashBudget.Adapter.Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Adapter.Example.Controllers;

[ApiController]
[Route("expenses")]
public class ExampleController(IAdapterService adapterService) : Controller
{
    [HttpGet("history")]
    public async Task<AdapterResponse> GetExpensesHistory(
        [FromQuery] string? continuationToken)
    {
        return await adapterService.GetExpensesHistory(continuationToken);
    }
}