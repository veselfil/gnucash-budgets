using GnuCashBudget.Application.Responses;
using MediatR;

namespace GnuCashBudget.Application.Requests;

public record GetBudgetsInRangeRequest(DateTime From, DateTime To) : IRequest<GetBudgetsInRangeResponse>;
