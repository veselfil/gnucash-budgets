using GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Feature.BudgetedAccounts.Budgeting;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ListAccounts.Response> ListAccounts()
    {
        return await _sender.Send(new ListAccounts.Request());
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<ListTransactionsInDateRange.Response> ListTransactionsInDateRange(
        [FromRoute] string accountId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        return await _sender.Send(new ListTransactionsInDateRange.Request(accountId,
            from, to));
    }
}