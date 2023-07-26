namespace GnuCashBudget.Application.Responses;

public record GetTransactionsForAccountInDateRangeResponse(
    string AccountId, IEnumerable<AccountTransactionResponse> Transactions);
