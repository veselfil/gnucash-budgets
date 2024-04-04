using System.Collections.Immutable;
using GnuCashBudget.Adapter.Abstractions;
using GnuCashBudget.Adapter.Abstractions.Models;
using GnuCashBudget.Adapter.Example.Models;

namespace GnuCashBudget.Adapter.Example;

// The Adapter makes the Adaptee's interface compatible with the Target's
// interface.
public class Adapter(Adaptee adaptee)  : IBankTarget
{
    public Task<ImmutableList<Expense>> GetExpensesHistoryAsync()
    {
        var rawExpenses = adaptee.GetExampleData();
        var expenses = rawExpenses.Select(MapToExpense).ToImmutableList();

        return Task.FromResult(expenses);
    }

    private Expense MapToExpense(ExampleExpense exampleExpense)
    {
        return new Expense
        {
            TransactionId = exampleExpense.TransactionId,
            Amount = exampleExpense.Amount,
            Currency = exampleExpense.Currency,
            Category = exampleExpense.Category,
            Description = exampleExpense.Description,
            Timestamp = exampleExpense.Timestamp,
            Merchant = exampleExpense.Merchant,
            PaymentMethod = exampleExpense.PaymentMethod,
            Location = exampleExpense.Location,
        };
    }
}