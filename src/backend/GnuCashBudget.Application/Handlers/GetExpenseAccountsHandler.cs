using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.GnuCashData.Abstractions;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class GetExpenseAccountsHandler: IRequestHandler<GetExpenseAccountsRequest, GetExpenseAccountsResponse>
{
    private readonly IExpenseAccountsRepository _expenseAccountsRepository;

    public GetExpenseAccountsHandler(IExpenseAccountsRepository expenseAccountsRepo)
    {
        _expenseAccountsRepository = expenseAccountsRepo;
    }

    public async Task<GetExpenseAccountsResponse> Handle(GetExpenseAccountsRequest request, CancellationToken cancellationToken)
    {
        var accounts = await _expenseAccountsRepository.GetAllExpenseAccounts();
        return new GetExpenseAccountsResponse
        {
            Accounts = accounts.Where(x => !request.BottomLevelOnly || !x.ChildAccounts.Any())
        };
    }
}