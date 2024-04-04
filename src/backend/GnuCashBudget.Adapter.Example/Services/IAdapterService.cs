using System.Collections.Immutable;
using GnuCashBudget.Adapter.Abstractions.Models;

namespace GnuCashBudget.Adapter.Example.Services;

public interface IAdapterService
{
    Task<ImmutableList<Expense>> GetExpensesHistory();
}