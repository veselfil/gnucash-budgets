using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Services;
using MediatR;

namespace GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;

public class ListAccounts
{
    public record Request : IRequest<Response>;
    public record Response(ImmutableList<Account> Accounts);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IGnuCashAccountsService _accountsService;
        
        public Handler(IGnuCashAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            return new Response(await _accountsService.GetAllAccounts());
        }
    }
}

