using GnuCashBudget.Application.Responses;
using MediatR;

namespace GnuCashBudget.Application.Requests;

public class GetBalancesRequest: IRequest<GetBalancesResponse>
{
}