using GnuCashBudget.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GnuCashBudget.Api.Controllers;

[ApiController]
[Route("account-transactions")]
public class TransactionsController: ControllerBase
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender)
    {
        _sender = sender;
    }
}