using System.Collections.Immutable;
using GnuCashBudget.Adapter.Example.Configuration;
using GnuCashBudget.Adapter.Example.Exceptions;
using GnuCashBudget.Adapter.Example.Models;
using Microsoft.Extensions.Options;

namespace GnuCashBudget.Adapter.Example;

// The Adaptee contains some useful behavior, but its interface is
// incompatible with the existing client code. The Adaptee needs some
// adaptation before the client code can use it.
public class Adaptee(IOptions<ExampleOptions> exampleOptions)
{
    private readonly ExampleOptions _exampleOptions = exampleOptions.Value;

    public ImmutableList<ExampleExpense> GetExampleData()
    {
        if (_exampleOptions.Expenses is null || _exampleOptions.Expenses.Count == 0)
        {
            throw new SourceDataEmptyException();
        }
        
        return _exampleOptions.Expenses.ToImmutableList();
    }
}