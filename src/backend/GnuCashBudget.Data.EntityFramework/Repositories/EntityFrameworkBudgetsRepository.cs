using GnuCashBudget.Data.Abstractions.Repositories;
using GnuCashBudget.Data.EntityFramework.Entities;
using GnuCashBudget.Data.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

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

    public async Task RemoveBudget(int budgetId)
    {
        var budgetOrNull = await _context.Budgets.SingleOrDefaultAsync(x => x.Id == budgetId);
        if (budgetOrNull != null)
        {
            _context.Remove(budgetOrNull);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Budget?> GetBudgetOrDefault(int budgetedAccountId, DateTime from, DateTime to)
    {
        return await _context.Budgets.FirstOrDefaultAsync(x => x.BudgetedAccountId == budgetedAccountId &&
                                                               x.StartDate == from &&
                                                               x.EndDate == to) 
        switch
        {
            null => null, 
            var e => MapToDomain(e)
        };
    }

    public async Task<IEnumerable<Budget>> GetBudgetsInTimeRange(DateTime from, DateTime to)
    {
        var entities = await _context.Budgets
            .Where(x => x.StartDate >= from && x.EndDate <= to)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Budget>> GetBudgetsForAccount(int budgetedAccountId)
    {
        var entities = await _context.Budgets
            .Where(x => x.BudgetedAccountId == budgetedAccountId)
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