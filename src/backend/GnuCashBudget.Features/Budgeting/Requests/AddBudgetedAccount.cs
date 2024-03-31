using GnuCashBudget.GnuCashData.EntityFramework.Services;
using GnuCashBudget.SharedDomain.Data;
using GnuCashBudget.SharedDomain.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;

public class AddBudgetedAccount
{
    public record Request(string AccountId): IRequest<Response>;
    public record Response(ErrorType ErrorType): ErrorResponseBase(ErrorType);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly BudgetsContext _context;
        private readonly IGnuCashAccountsService _accountsService;
        
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var account = await _accountsService.Find(request.AccountId);
            if (account == null)
            {
                return new Response(ErrorType.NotFound);
            }

            var budgetedAccount = await _context.BudgetedAccounts.SingleOrDefaultAsync(x => x.AccountGuid == request.AccountId);
            if (budgetedAccount != null)
            {
                return new Response(ErrorType.AlreadyExists);
            }

            _context.BudgetedAccounts.Add(new BudgetedAccountEntity { AccountGuid = request.AccountId });
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(ErrorType.None);
        }
    }
}