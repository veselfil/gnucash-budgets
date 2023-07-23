using GnuCashBudget.Application.Responses;
using MediatR;

namespace GnuCashBudget.Application.Requests;

public record GetExpenseAccountsRequest(bool BottomLevelOnly) : IRequest<GetExpenseAccountsResponse>;
