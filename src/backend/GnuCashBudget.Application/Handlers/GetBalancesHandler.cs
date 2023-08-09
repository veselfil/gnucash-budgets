using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Models;
using GnuCashBudget.GnuCashData.Abstractions;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class GetBalancesHandler: IRequestHandler<GetBalancesRequest, GetBalancesResponse>
{
    private readonly IBudgetedAccountRepository _budgetedAccountsRepository;
    private readonly IAccountTransactionsRepository _accountTransactions;
    private readonly IBudgetsRepository _budgetsRepository;
    private readonly IExpenseAccountsRepository _accountRepository;

    public GetBalancesHandler(
        IBudgetedAccountRepository budgetedAccountsRepository, 
        IAccountTransactionsRepository accountTransactions, 
        IBudgetsRepository budgetsRepository,
        IExpenseAccountsRepository accountRepository)
    {
        _budgetedAccountsRepository = budgetedAccountsRepository;
        _accountTransactions = accountTransactions;
        _budgetsRepository = budgetsRepository;
        _accountRepository = accountRepository;
    }

    public async Task<GetBalancesResponse> Handle(GetBalancesRequest request, CancellationToken cancellationToken)
    {
        var budgetedAccounts = await _budgetedAccountsRepository.GetAll();
        var balances = new List<BalanceResponse>();
        
        foreach (var account in budgetedAccounts)
        {
            balances.Add(await GetBalanceForAccount(account));
        }

        return new GetBalancesResponse { Balances = balances };
    }

    private async Task<BalanceResponse> GetBalanceForAccount(BudgetedAccount account)
    {
        var budgets = (await _budgetsRepository.GetBudgetsForAccount(account.Id)).ToList();
        var budgetPeriodsRaw = budgets
            .Select(x => (Start: x.ValidFrom, End: x.ValidTo.AddMonths(1).AddDays(-1)))
            .OrderBy(x => x.Start);

        var gcAccount = await _accountRepository.Find(account.AccountGuid);
        if (gcAccount == null)
        {
            throw new InvalidOperationException($"Could not find GNU Cash account with ID {account.AccountGuid}");
        }
        
        var transactionsSum = 0M;
        foreach (var (start, end) in budgetPeriodsRaw)
        {
            var transactionsInPeriod = _accountTransactions.GetTransactionsForAccountInDateRange(
                gcAccount.Id, start, end);

            transactionsSum += await _accountTransactions.GetSumOfTransactions(gcAccount.Id, start, end);
        }

        var budgetsSum = budgets.Sum(x => x.Amount);
        return new BalanceResponse(
            gcAccount.FullName,
            account.AccountGuid,
            account.Id,
            budgetsSum - transactionsSum,
            gcAccount.CurrencyCode);
    }
}