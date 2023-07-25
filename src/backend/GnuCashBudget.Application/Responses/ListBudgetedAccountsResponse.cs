using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.Application.Responses;

public class ListBudgetedAccountsResponse
{
    public IEnumerable<BudgetedAccountResponse> Accounts { get; set; }
}