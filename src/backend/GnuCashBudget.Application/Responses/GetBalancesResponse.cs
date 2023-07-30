namespace GnuCashBudget.Application.Responses;

public class GetBalancesResponse
{
    public IEnumerable<BalanceResponse> Balances { get; set; }
}

public record BalanceResponse(string AccountName, string AccountId, int BudgetedAccountId, decimal Balance);
