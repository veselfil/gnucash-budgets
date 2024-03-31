using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.GnuCashData.EntityFramework;

public class GnuCashContext : DbContext
{
    public GnuCashContext(DbContextOptions<GnuCashContext> options) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BookEntity>().HasNoKey();
        modelBuilder.Entity<AccountEntity>()
            .Property(x => x.Type)
            .HasConversion( // the casing needs to be fixed here
                accountType => accountType.ToString().ToUpper(),
                s => Enum.Parse<AccountType>(s, true));
    }

    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<BookEntity> Books { get; set; }
    public DbSet<CommodityEntity> Commodities { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<SplitEntity> Splits { get; set; }
}