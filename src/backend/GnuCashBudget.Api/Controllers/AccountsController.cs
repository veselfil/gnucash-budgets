using System.Collections.Immutable;
using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController
{
    private readonly IAccountRepository _accountsRepository;
    private readonly ISender _sender;

    public AccountsController(IAccountRepository accountsRepository, ISender sender)
    {
        _accountsRepository = accountsRepository;
        _sender = sender;
    }

    [HttpGet]
    public async Task<ImmutableList<Account>> GetAllAccounts()
    {
        return await _accountsRepository.GetAllAccounts();
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<GetTransactionsForAccountInDateRangeResponse> GetTransactionsInDateRange(
        [FromRoute] string accountId,
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateTo)
    {
        return await _sender.Send(new GetTransactionsForAccountInDateRangeRequest(
            accountId, dateFrom, dateTo));
    }
}