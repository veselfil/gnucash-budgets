using GnuCashBudget.SharedDomain.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.SharedDomain.Data;

public class BudgetsContext: DbContext
{
    public DbSet<BudgetedAccountEntity> BudgetedAccounts { get; set; }
    public DbSet<BudgetEntity> Budgets { get; set; }

    public BudgetsContext(DbContextOptions<BudgetsContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }
}