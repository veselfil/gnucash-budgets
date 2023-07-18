using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Abstract = GnuCashBudget.GnuCashData.Abstractions.Models;
using AccountType = GnuCashBudget.GnuCashData.EntityFramework.Entities.AccountType;

namespace GnuCashBudget.GnuCashData.EntityFramework;

public class EntityFrameworkAccountsRepository: IAccountRepository
{
    private readonly GnuCashContext _dataContext;

    public EntityFrameworkAccountsRepository(GnuCashContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ImmutableList<Account>> GetAllAccounts()
    {
        var allAccounts = await _dataContext.Accounts.ToListAsync();
        return allAccounts
            .Select(MapAccount)
            .ToImmutableList();
    }

    public async Task<ImmutableList<Account>> GetAccountsByType(Abstract.AccountType type)
    {
        var accountType = (AccountType)type;
        var accounts = await _dataContext.Accounts
            .Where(x => x.Type == accountType)
            .ToListAsync();

        return accounts.Select(MapAccount).ToImmutableList();
    }

    private Account MapAccount(AccountEntity entity)
        => new (entity.Name, entity.CommodityId, (Abstract.AccountType)entity.Type);
}