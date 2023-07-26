using GnuCashBudget.Application.Responses;
using MediatR;

namespace GnuCashBudget.Application.Requests;

public record GetTransactionsForAccountInDateRangeRequest(
    string AccountId, DateTime From, DateTime To): IRequest<GetTransactionsForAccountInDateRangeResponse>;