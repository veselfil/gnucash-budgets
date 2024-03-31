using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;

[ApiController]
[Route("budgeted-accounts")]
public class BudgetedAccountsController: ControllerBase
{
    [HttpPost]
    public async Task<AddBudgetedAccount.Response> AddBudgetedAccount(
        [FromServices] ISender sender,
        [FromBody] AddBudgetedAccount.Request request)
    {
        return await sender.Send(request);
    }

    [HttpGet]
    public async Task<ListBudgetedAccounts.Response> ListBudgetedAccounts(
        [FromServices] ISender sender)
    {
        return await sender.Send(new ListBudgetedAccounts.Request());
    }
}