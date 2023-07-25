using GnuCashBudget.Application.Requests;
using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Models;
using GnuCashBudget.GnuCashData.Abstractions;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class AddBudgetedAccountHandler: IRequestHandler<AddBudgetedAccountRequest>
{
    private readonly IBudgetedAccountRepository _budgetedAccountRepository;
    private readonly IAccountRepository _accountRepository;

    public AddBudgetedAccountHandler(IBudgetedAccountRepository budgetedAccountRepository, IAccountRepository accountRepository)
    {
        _budgetedAccountRepository = budgetedAccountRepository;
        _accountRepository = accountRepository;
    }

    public async Task Handle(AddBudgetedAccountRequest request, CancellationToken cancellationToken)
    {
        if (await _accountRepository.Find(request.AccountGuid) == null)
            throw new InvalidOperationException($"Account with id {request.AccountGuid} does not exist");

        await _budgetedAccountRepository.Create(new BudgetedAccount(0, request.AccountGuid));
    }
}