using System.Collections.Immutable;
using GnuCashBudget.Adapter.Abstractions;
using GnuCashBudget.Adapter.Abstractions.Enums;
using GnuCashBudget.Adapter.Abstractions.Models;
using GnuCashBudget.Adapter.Example.Models;

namespace GnuCashBudget.Adapter.Example;

// The Adapter makes the Adaptee's interface compatible with the Target's
// interface.
public class Adapter(Adaptee adaptee) : IBankTarget
{
    public Task<AdapterResponse> GetExpensesHistoryAsync(string? continuationToken)
    {
        var rawExpenses = adaptee.GetExampleData();

        var expenses = FilterExpenses(rawExpenses, continuationToken)
            .OrderBy(x => x.Timestamp)
            .ToImmutableList();
        
        var newContinuationToken = expenses.IsEmpty
            ? continuationToken 
            : EncodeContinuationToken(expenses.Last().Timestamp);
        
        var response = new AdapterResponse
        {
            ContinuationToken = newContinuationToken,
            Expenses = expenses.Select(MapToExpense),
        };

        return Task.FromResult(response);
    }

    private Expense MapToExpense(ExampleExpense exampleExpense)
    {
        return new Expense
        {
            Amount = exampleExpense.Amount,
            Currency = exampleExpense.Currency,
            Category = exampleExpense.Category,
            Description = exampleExpense.Description,
            Timestamp = exampleExpense.Timestamp,
            Merchant = exampleExpense.Merchant,
            PaymentMethod = ParsePaymentMethod(exampleExpense.PaymentMethod),
            Location = exampleExpense.Location,
        };
    }

    private PaymentMethod? ParsePaymentMethod(string? value)
    {
        if (Enum.TryParse<PaymentMethod>(value, out var result))
        {
            return result;
        }

        return null;
    }

    private DateTime DecodeContinuationToken(string continuationToken)
    {
        var base64EncodedBytes = Convert.FromBase64String(continuationToken);
        var base64String = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

        return DateTime.Parse(base64String);
    }

    private string EncodeContinuationToken(string timestamp)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(timestamp);
        return Convert.ToBase64String(plainTextBytes);
    }

    private IEnumerable<ExampleExpense> FilterExpenses(ImmutableList<ExampleExpense> expenses, string? continuationToken)
    {
        var dateTime = continuationToken is not null ? DecodeContinuationToken(continuationToken) : DateTime.MinValue;
        
        return expenses
            .Where(x => DateTime.Parse(x.Timestamp) > dateTime);
    }
}