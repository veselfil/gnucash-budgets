using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class GetExpenseAccountsHandler: IRequestHandler<GetExpenseAccountsRequest, GetExpenseAccountsResponse>
{
    private readonly IAccountRepository _accountsRepo;

    public GetExpenseAccountsHandler(IAccountRepository accountsRepo)
    {
        _accountsRepo = accountsRepo;
    }

    public async Task<GetExpenseAccountsResponse> Handle(GetExpenseAccountsRequest request, CancellationToken cancellationToken)
    {
        return new GetExpenseAccountsResponse
        {
            Accounts = await _accountsRepo.GetAccountsByType(AccountType.Expense)
        };
    }
}