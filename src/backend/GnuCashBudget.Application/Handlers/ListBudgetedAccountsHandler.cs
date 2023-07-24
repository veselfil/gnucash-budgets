using System.Collections.Immutable;
using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Models;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class ListBudgetedAccountsHandler: IRequestHandler<ListBudgetedAccountsRequest, ListBudgetedAccountsResponse>
{
    private readonly IBudgetedAccountRepository _budgetedAccountsRepository;
    private readonly IAccountRepository _accountRepository;

    public ListBudgetedAccountsHandler(IBudgetedAccountRepository budgetedAccountsRepository, IAccountRepository accountRepository)
    {
        _budgetedAccountsRepository = budgetedAccountsRepository;
        _accountRepository = accountRepository;
    }

    public async Task<ListBudgetedAccountsResponse> Handle(ListBudgetedAccountsRequest request, CancellationToken cancellationToken)
    {
        var budgetedAccounts = await _budgetedAccountsRepository.GetAll();
        var budgetedAccountIds = budgetedAccounts.ToImmutableDictionary(x => x.AccountGuid, x => x);
        var allAccounts = await _accountRepository.GetAccountsByType(AccountType.Expense);

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
                .Select(x => new BudgetedAccountResponse(x.BudgetedAccount.Id, x.Account.Id, x.Account.FullName))
        };
    }
}