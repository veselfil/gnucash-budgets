using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using GnuCashBudget.GnuCashData.Abstractions;
using MediatR;

namespace GnuCashBudget.Application.Handlers;

public class GetTransactionsForAccountInDateRangeHandler: 
    IRequestHandler<GetTransactionsForAccountInDateRangeRequest, GetTransactionsForAccountInDateRangeResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountTransactionsRepository _accountTransactionsRepository;

    public GetTransactionsForAccountInDateRangeHandler(
        IAccountRepository accountRepository,
        IAccountTransactionsRepository accountTransactionsRepository)
    {
        _accountRepository = accountRepository;
        _accountTransactionsRepository = accountTransactionsRepository;
    }

    public async Task<GetTransactionsForAccountInDateRangeResponse> Handle(
        GetTransactionsForAccountInDateRangeRequest request, 
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.Find(request.AccountId);
        if (account == null)
        {
            throw new InvalidOperationException($"Account with ID {request.AccountId} was not found");
        }

        var transactions = await _accountTransactionsRepository.GetTransactionsForAccountInDateRange(
            account, request.From, request.To);

        return new GetTransactionsForAccountInDateRangeResponse(
            account.Id,
            transactions.Select(t => new AccountTransactionResponse(t.Date, t.TransactionAmount)));
    }
}