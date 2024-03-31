using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Services;
using GnuCashBudget.SharedDomain.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;

public class ListBudgetedAccounts
{
    public record Request : IRequest<Response>;

    public record Response(ImmutableList<BudgetedAccountResponse> Accounts);

    public record BudgetedAccountResponse(
        int BudgetedAccountId,
        string AccountId,
        string FullName,
        string CurrencyCode);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IGnuCashAccountsService _accountsService;
        private readonly BudgetsContext _context;

        public Handler(IGnuCashAccountsService accountsService, BudgetsContext context)
        {
            _accountsService = accountsService;
            _context = context;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var budgetedAccounts = await _context.BudgetedAccounts
                .ToListAsync(cancellationToken);

            var budgetedAccountIds = budgetedAccounts.ToImmutableDictionary(x => x.AccountGuid, x => x);
            var allAccounts = await _accountsService.GetAccountsByType(AccountType.Expense);

            var result = allAccounts
                .Where(x => budgetedAccountIds.ContainsKey(x.Id))
                .Select(x => (Account: x, BudgetedAccount: budgetedAccountIds[x.Id]))
                .Select(x => new BudgetedAccountResponse(
                    x.BudgetedAccount.Id,
                    x.Account.Id,
                    x.Account.FullName,
                    x.Account.Commodity))
                .ToImmutableList();

            return new Response(result);
        }
    }
}