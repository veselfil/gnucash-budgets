using System.Collections.Immutable;
using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Models;
using GnuCashBudget.GnuCashData.Abstractions;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class ListBudgetedAccountsHandler: IRequestHandler<ListBudgetedAccountsRequest, ListBudgetedAccountsResponse>
{
    private readonly IBudgetedAccountRepository _budgetedAccountsRepository;
    private readonly IExpenseAccountsRepository _accountRepository;

    public ListBudgetedAccountsHandler(IBudgetedAccountRepository budgetedAccountsRepository, IExpenseAccountsRepository accountRepository)
    {
        _budgetedAccountsRepository = budgetedAccountsRepository;
        _accountRepository = accountRepository;
    }

    public async Task<ListBudgetedAccountsResponse> Handle(ListBudgetedAccountsRequest request, CancellationToken cancellationToken)
    {
        var budgetedAccounts = await _budgetedAccountsRepository.GetAll();
        var budgetedAccountIds = budgetedAccounts.ToImmutableDictionary(x => x.AccountGuid, x => x);
        var allAccounts = await _accountRepository.GetAllExpenseAccounts();

        BudgetedAccount? GetBudgetedAccount(string accountGuid)
        {
            if (budgetedAccountIds.TryGetValue(accountGuid, out var budgetedAccount))
                return budgetedAccount;
            
            return null;
        }
        
        return new ListBudgetedAccountsResponse
        {
            Accounts = allAccounts
                .Select(x => new { Account = x, BudgetedAccount = GetBudgetedAccount(x.Id) })
                .Where(x => x.BudgetedAccount != null)
                .Select(x => new BudgetedAccountResponse(
                    x.BudgetedAccount.Id, x.Account.Id, x.Account.FullName, x.Account.CurrencyCode))
        };
    }
}