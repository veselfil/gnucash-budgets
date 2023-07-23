using System.Runtime.CompilerServices;
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
        var accounts = await _accountsRepo.GetAccountsByType(AccountType.Expense);
        return new GetExpenseAccountsResponse
        {
            Accounts = accounts.Where(x => !request.BottomLevelOnly || !x.ChildAccounts.Any())
        };
    }
}