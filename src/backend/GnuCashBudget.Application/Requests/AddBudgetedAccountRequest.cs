using MediatR;

namespace GnuCashBudget.Application.Requests;

public record AddBudgetedAccountRequest(string AccountGuid) : IRequest;
