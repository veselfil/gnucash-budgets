namespace GnuCashBudget.GnuCashData.Generator.Configuration;

public class GeneratorOptions
{
    public const string Generator = "Generator";
    
    public string DatabaseFile { get; set; }
    
    public int IncomeAmount { get; set; }
    
    public int ExpenseAccountCount { get; set; }
    public int MaxPriceOfOneExpense { get; set; }
    public int PercentageToExpense { get; set; }
    
    public DateTime ExpensesFrom { get; set; }
    public DateTime ExpensesTo { get; set; }
}