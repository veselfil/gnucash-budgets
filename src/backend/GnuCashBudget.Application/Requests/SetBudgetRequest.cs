using MediatR;

namespace GnuCashBudget.Application.Requests;

public record SetBudgetRequest(int BudgetId, decimal Amount, int AccountId, DateTime Date) : IRequest;
