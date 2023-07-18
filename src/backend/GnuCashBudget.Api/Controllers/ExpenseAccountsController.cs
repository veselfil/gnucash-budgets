using GnuCashBudget.Application.Requests;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Api.Controllers;

[ApiController]
[Route("expense-accounts")]
public class ExpenseAccountsController : ControllerBase
{
    private readonly ISender _sender;

    public ExpenseAccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IEnumerable<Account>> GetAllExpenseAccounts()
    {
        var response = await _sender.Send(new GetExpenseAccountsRequest());
        return response.Accounts;
    }
}