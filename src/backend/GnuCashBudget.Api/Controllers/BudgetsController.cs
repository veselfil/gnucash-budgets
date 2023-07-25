using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Api.Controllers;

[ApiController]
[Route("budgets")]
public class BudgetsController: Controller
{
    private readonly ISender _sender;

    public BudgetsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPut]
    public async Task SetBudget([FromBody] SetBudgetRequest request)
    {
        await _sender.Send(request);
    }

    [HttpGet]
    public async Task<GetBudgetsInRangeResponse> GetBudgetsInRange(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        return await _sender.Send(new GetBudgetsInRangeRequest(fromDate, toDate));
    }
}