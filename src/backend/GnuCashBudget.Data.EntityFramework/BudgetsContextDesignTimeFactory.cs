using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GnuCashBudget.Data.EntityFramework;

public class BudgetsContextDesignTimeFactory: IDesignTimeDbContextFactory<BudgetsContext>
{
    public BudgetsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BudgetsContext>();
        optionsBuilder.UseSqlite();
        return new BudgetsContext(optionsBuilder.Options);
    }
}