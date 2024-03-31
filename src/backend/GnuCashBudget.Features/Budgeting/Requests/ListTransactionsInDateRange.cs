using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.EntityFramework.Services;
using MediatR;

namespace GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;

public static class ListTransactionsInDateRange
{
    public record Request(
        string AccountId,
        DateTime From,
        DateTime To
    ): IRequest<Response>;
    
    public record Response(string AccountId, ImmutableList<AccountTransactionResponse> Transactions);
    public record AccountTransactionResponse(DateTime Date, decimal Amount, string Description);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IGnuCashAccountsService _gnuCashAccountsService;

        public Handler(IGnuCashAccountsService gnuCashAccountsService)
        {
            _gnuCashAccountsService = gnuCashAccountsService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var account = await _gnuCashAccountsService.Find(request.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException($"Account with ID {request.AccountId} was not found");
            }

            var transactions = await _gnuCashAccountsService.GetTransactionsForAccountInDateRange(
                account.Id, request.From, request.To);

            return new Response(
                account.Id,
                transactions
                    .Select(t => new AccountTransactionResponse(t.Date, t.TransactionAmount, t.Description))
                    .ToImmutableList());
        }
    }
}