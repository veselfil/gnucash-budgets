using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Entities;
using GnuCashBudget.Data.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.Data.EntityFramework.Repositories;

public class EntityFrameworkBudgetedAccountsRepository: IBudgetedAccountRepository
{
    private readonly BudgetsContext _context;

    public EntityFrameworkBudgetedAccountsRepository(BudgetsContext context)
    {
        _context = context;
    }

    public async Task Create(BudgetedAccount account)
    {
        _context.BudgetedAccounts.Add(new BudgetedAccountEntity { AccountGuid = account.AccountGuid });
        
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BudgetedAccount>> GetAll()
    {
        var entities = await _context.BudgetedAccounts.ToListAsync();
        return entities.Select(x => new BudgetedAccount(x.Id, x.AccountGuid)).ToList();
    }
}