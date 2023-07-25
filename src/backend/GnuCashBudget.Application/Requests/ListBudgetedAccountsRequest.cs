using GnuCashBudget.Application.Responses;
using MediatR;

namespace GnuCashBudget.Application.Requests;

public record ListBudgetedAccountsRequest : IRequest<ListBudgetedAccountsResponse>;
