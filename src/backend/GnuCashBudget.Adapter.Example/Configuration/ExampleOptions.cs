using GnuCashBudget.Adapter.Example.Models;

namespace GnuCashBudget.Adapter.Example.Configuration;

public class ExampleOptions
{
    public const string Example = "ExampleOptions";
    
    public List<ExampleExpense> Expenses { get; set; }
}