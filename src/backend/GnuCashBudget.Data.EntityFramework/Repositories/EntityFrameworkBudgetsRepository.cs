using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Entities;
using GnuCashBudget.Data.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.Data.EntityFramework.Repositories;

public class EntityFrameworkBudgetsRepository: IBudgetsRepository
{
    private readonly BudgetsContext _context;

    public EntityFrameworkBudgetsRepository(BudgetsContext context)
    {
        _context = context;
    }

    public async Task UpsertBudget(Budget budget)
    {
        if (await _context.Budgets.AnyAsync(x => x.Id == budget.Id))
        {
            _context.Update(MapToEntity(budget));
        }
        else
        {
            await _context.AddAsync(MapToEntity(budget));
        }
    
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Budget>> GetBudgetsInTimeRange(DateTime from, DateTime to)
    {
        var entities = await _context.Budgets
            .Where(x => x.StartDate >= from && x.EndDate <= to)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    private Budget MapToDomain(BudgetEntity entity)
    {
        return new Budget(entity.Id, entity.BudgetedAccountId, 
            entity.StartDate, entity.EndDate, entity.Amount);
    }
    
    private BudgetEntity MapToEntity(Budget budget)
    {
        return new BudgetEntity
        {
            Id = budget.Id, BudgetedAccountId = budget.BudgetedAccount, Amount = budget.Amount,
            EndDate = budget.ValidTo, StartDate = budget.ValidFrom
        };
    }
}