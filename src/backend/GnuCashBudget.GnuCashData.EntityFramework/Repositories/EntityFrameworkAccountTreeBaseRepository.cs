using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Abstract = GnuCashBudget.GnuCashData.Abstractions.Models;
using AccountType = GnuCashBudget.GnuCashData.EntityFramework.Entities.AccountType;

namespace GnuCashBudget.GnuCashData.EntityFramework.Repositories;

public abstract class EntityFrameworkAccountTreeBaseRepository(GnuCashContext context)
{
    protected GnuCashContext Context { get; } = context;

    protected ImmutableList<AccountWithCommodity> FlattenAccountTree(ImmutableList<AccountWithCommodity> accountsTree)
    {
        var accounts = new List<AccountWithCommodity>(accountsTree);
        foreach (var account in accountsTree)
        {
            accounts.AddRange(FlattenAccountTree(account.Children.ToImmutableList()));
        }

        return accounts.ToImmutableList();
    }

    protected async Task<ImmutableList<AccountWithCommodity>> GetFullAccountTree(bool includeRootAccount = false)
    {
        var accounts = await Context.Accounts
            .Join(Context.Commodities, account => account.CommodityId, commodity => commodity.Id,
                (account, commodity) => new { Account = account, Commodity = commodity })
            .ToListAsync();
        
        var rootAccount = accounts.Single(x => x is { Account.Type: AccountType.Root, Account.Name: "Root Account" });
        var rootAccountWrapper = new AccountWithCommodity
        {
            Account = MapAccount(rootAccount.Account),
            Commodity = MapCommodity(rootAccount.Commodity)
        };

        AssignChildren(rootAccountWrapper);
        BuildFullNames(rootAccountWrapper);

        if (includeRootAccount)
        {
            return new List<AccountWithCommodity>() { rootAccountWrapper }.ToImmutableList();
        }
        
        return rootAccountWrapper.Children
            .ToImmutableList();

        void AssignChildren(AccountWithCommodity account)
        {
            account.Children = accounts
                .Where(x => x.Account.ParentId == account.Account.Id)
                .Select(x => new AccountWithCommodity { Account = MapAccount(x.Account), Commodity = MapCommodity(x.Commodity), Parent = account })
                .ToList();

            foreach (var child in account.Children)
            {
                AssignChildren(child);
            }
        }

        void BuildFullNames(AccountWithCommodity account, string name = "")
        {
            if (account.Account.AccountType != Abstract.AccountType.Root)
                account.FullName = $"{name}:{account.Account.Name}";

            foreach (var childAccount in account.Children)
            {
                BuildFullNames(childAccount, account.FullName);
            }
        }
    }

    protected Account MapAccount(AccountEntity entity)
        => new(entity.Id,
            entity.Name,
            "UNKNOWN",
            "UNKNOWN",
            (Abstract.AccountType)entity.Type,
            ImmutableList<Account>.Empty,
            entity.ParentId);

    private Commodity MapCommodity(CommodityEntity entity)
        => new(entity.Id,
            entity.Namespace,
            entity.Mnemonic,
            entity.Fraction);

    protected class AccountWithCommodity
    {
        public Account? Account { get; init; }
        
        public Commodity? Commodity { get; init; }
        public List<AccountWithCommodity> Children { get; set; } = new();

        public string? FullName { get; set; }
        public AccountWithCommodity? Parent { get; set; }
    }
}