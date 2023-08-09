using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.Application.Responses;

public class GetExpenseAccountsResponse
{
    public IEnumerable<ExpenseAccount> Accounts { get; set; }
}

