using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.Data.Abstractions.Repositories;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class GetBudgetsInRangeHandler : IRequestHandler<GetBudgetsInRangeRequest, GetBudgetsInRangeResponse>
{
    private readonly IBudgetsRepository _budgetsRepository;

    public GetBudgetsInRangeHandler(IBudgetsRepository budgetsRepository)
    {
        _budgetsRepository = budgetsRepository;
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
}