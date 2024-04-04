using System.Collections.Immutable;
using GnuCashBudget.Adapter.Abstractions.Models;
using GnuCashBudget.Adapter.Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Adapter.Example.Controllers;

[ApiController]
[Route("expenses")]
public class ExampleController(IAdapterService adapterService) : Controller
{
    [HttpGet("history")]
    public async Task<ImmutableList<Expense>> GetExpensesHistory(
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        return await adapterService.GetExpensesHistory();
    }
}