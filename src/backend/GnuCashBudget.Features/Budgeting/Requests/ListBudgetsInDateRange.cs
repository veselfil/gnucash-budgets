using GnuCashBudget.SharedDomain.Data;
using MediatR;

namespace GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;

public class ListBudgetsInDateRange
{
    public record Request(DateTime From, DateTime To): IRequest<Response>;
    public record Response;
    
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly BudgetsContext _context;

        public Handler(BudgetsContext context)
        {
            _context = context;
        }

        public async Task<GetBudgetsInRangeResponse> Handle(GetBudgetsInRangeRequest request,
            CancellationToken cancellationToken)
        {
            var budgets = await _budgetsRepository.GetBudgetsInTimeRange(request.From, request.To);
            return new GetBudgetsInRangeResponse
            {
                Budgets = budgets.Select(x => new BudgetResponse(x.Id, x.BudgetedAccount, x.Amount, x.ValidFrom, x.ValidTo))
            };
        }

        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var budgets = await _context.Budgets
                .Where(x => x.StartD)
        }
    }
}