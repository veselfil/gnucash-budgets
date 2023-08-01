using GnuCashBudget.Data.EntityFramework.Models;

namespace GnuCashBudget.Data.Abstractions.Repositories;

public interface IBudgetsRepository
{
    Task UpsertBudget(Budget budget);
    Task RemoveBudget(int budgetId);
    
    /// <summary>
    /// Returns the budget with *exactly* matches the search params (there is no interval matching etc.)
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<Budget?> GetBudgetOrDefault(int accountId, DateTime from, DateTime to);    
    Task<IEnumerable<Budget>> GetBudgetsInTimeRange(DateTime from, DateTime to);
    Task<IEnumerable<Budget>> GetBudgetsForAccount(int budgetedAccountId);
}