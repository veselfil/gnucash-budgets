using GnuCashBudget.Data.EntityFramework.Models;

namespace GnuCashBudget.Data.Abstractions.Repositories;

public interface IBudgetedAccountRepository
{
    Task Create(BudgetedAccount account);
    Task<IEnumerable<BudgetedAccount>> GetAll();
}