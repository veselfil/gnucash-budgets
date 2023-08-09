using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Abstract = GnuCashBudget.GnuCashData.Abstractions.Models;
using AccountType = GnuCashBudget.GnuCashData.EntityFramework.Entities.AccountType;

namespace GnuCashBudget.GnuCashData.EntityFramework.Repositories;

public class EntityFrameworkAccountsRepository : EntityFrameworkAccountTreeBaseRepository, IAccountRepository
{
    public EntityFrameworkAccountsRepository(GnuCashContext dataContext): base(dataContext)
    {
    }

    public async Task<Account?> Find(string accountId)
    {
        // TODO: This is fucking horrible
        var accountTree = await this.GetFullAccountTree();
        var flatList = this.FlattenAccountTree(accountTree);

        return WrapperToAccount(flatList.FirstOrDefault(x => x.Account.Id == accountId));
    }

    public async Task<ImmutableList<Account>> GetAllAccounts()
    {
        return FlattenAccountTree(await GetFullAccountTree())
            .Select(WrapperToAccount)
            .ToImmutableList();
    }

    public async Task<ImmutableList<Account>> GetAccountsByType(Abstract.AccountType type)
    {
        var accountTree = await GetFullAccountTree();
        var accounts = FlattenAccountTree(accountTree);

        return accounts.Where(x => x.Account.AccountType == type)
            .Select(WrapperToAccount)
            .ToImmutableList();
    }

    Account WrapperToAccount(AccountWithCommodity wrapper)
    {
        return wrapper.Account with
        {
            ChildAccounts = wrapper.Children.Select(WrapperToAccount).ToImmutableList(),
            FullName = wrapper.FullName.Substring(1),
            Commodity = wrapper.Commodity.Mnemonic
        };
    }
}