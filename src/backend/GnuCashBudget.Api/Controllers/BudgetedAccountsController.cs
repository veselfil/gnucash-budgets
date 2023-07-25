using System.Net;
using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Api.Controllers;

[ApiController]
[Route("budgeted-accounts")]
public class BudgetedAccountsController: Controller
{
    private readonly ISender _sender;

    public BudgetedAccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ListBudgetedAccountsResponse> ListBudgetedAccounts()
    {
        return await _sender.Send(new ListBudgetedAccountsRequest());
    }

    [HttpPost]
    public async Task AddBudgetedAccount([FromBody] AddBudgetedAccountRequest request)
    {
        await _sender.Send(request);
    }
}