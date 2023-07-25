using GnuCashBudget.Application.Requests;
using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Models;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class SetBudgetHandler: IRequestHandler<SetBudgetRequest>
{
    private readonly IBudgetsRepository _budgetsRepository;

    public SetBudgetHandler(IBudgetsRepository budgetsRepository)
    {
        _budgetsRepository = budgetsRepository;
    }

    public async Task Handle(SetBudgetRequest request, CancellationToken cancellationToken)
    {
        var budget = new Budget(request.BudgetId, request.AccountId, request.Date, request.Date, request.Amount);
        await _budgetsRepository.UpsertBudget(budget);
    }
}