using GnuCashBudget.Data.EntityFramework.Models;

namespace GnuCashBudget.Data.Abstractions.Repositories;

public interface IBudgetsRepository
{
    Task UpsertBudget(Budget budget);
    Task<IEnumerable<Budget>> GetBudgetsInTimeRange(DateTime from, DateTime to);
    Task<IEnumerable<Budget>> GetBudgetsForAccount(int budgetedAccountId);
}