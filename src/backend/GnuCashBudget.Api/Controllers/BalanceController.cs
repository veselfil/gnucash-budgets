using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Api.Controllers;

[ApiController]
[Route("balances")]
public class BalanceController: Controller
{
    private readonly ISender _sender;

    public BalanceController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<GetBalancesResponse> GetBalances()
    {
        return await _sender.Send(new GetBalancesRequest());
    }
}