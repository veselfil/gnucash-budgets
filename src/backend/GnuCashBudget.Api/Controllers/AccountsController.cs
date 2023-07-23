using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController
{
    private readonly IAccountRepository _accountsRepository;

    public AccountsController(IAccountRepository accountsRepository)
    {
        _accountsRepository = accountsRepository;
    }

    [HttpGet]
    public async Task<ImmutableList<Account>> GetAllAccounts()
    {
        return await _accountsRepository.GetAllAccounts();
    }
}