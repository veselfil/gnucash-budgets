using System.Collections.Immutable;
using System.Xml;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Abstract = GnuCashBudget.GnuCashData.Abstractions.Models;
using AccountType = GnuCashBudget.GnuCashData.EntityFramework.Entities.AccountType;

namespace GnuCashBudget.GnuCashData.EntityFramework;

public class EntityFrameworkAccountsRepository : IAccountRepository
{
    private readonly GnuCashContext _dataContext;

    public EntityFrameworkAccountsRepository(GnuCashContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ImmutableList<Account>> GetAllAccounts()
    {
        return await GetFullAccountTree();
    }

    public async Task<ImmutableList<Account>> GetAccountsByType(Abstract.AccountType type)
    {
        var accountTree = await GetFullAccountTree();
        var accounts = FlattenAccountTree(accountTree);

        return accounts.Where(x => x.AccountType == type).ToImmutableList();
    }

    private ImmutableList<Account> FlattenAccountTree(ImmutableList<Account> accountsTree)
    {
        var accounts = new List<Account>(accountsTree);
        foreach (var account in accountsTree)
        {
            accounts.AddRange(FlattenAccountTree(account.ChildAccounts));
        }

        return accounts.ToImmutableList();
    }

    private async Task<ImmutableList<Account>> GetFullAccountTree()
    {
        var accounts = await _dataContext.Accounts.ToListAsync();
        var rootAccount = accounts.Single(x => x is { Type: AccountType.Root, Name: "Root Account" });
        var rootAccountWrapper = new AccountWithChildren { Account = MapAccount(rootAccount) };

        AssignChildren(rootAccountWrapper);
        BuildFullNames(rootAccountWrapper);

        return rootAccountWrapper.Children
            .Select(WrapperToAccount)
            .ToImmutableList();

        void AssignChildren(AccountWithChildren account)
        {
            account.Children = accounts
                .Where(x => x.ParentId == account.Account.Id)
                .Select(x => new AccountWithChildren { Account = MapAccount(x), Parent = account })
                .ToList();

            foreach (var child in account.Children)
            {
                AssignChildren(child);
            }
        }

        void BuildFullNames(AccountWithChildren account, string name = "")
        {
            if (account.Account.AccountType != Abstract.AccountType.Root)
                account.FullName = $"{name}:{account.Account.Name}";
            
            foreach (var childAccount in account.Children)
            {
                BuildFullNames(childAccount, account.FullName);
            }
        }

        Account WrapperToAccount(AccountWithChildren wrapper)
        {
            return wrapper.Account with
            {
                ChildAccounts = wrapper.Children.Select(WrapperToAccount).ToImmutableList(),
                FullName = wrapper.FullName.Substring(1)
            };
        }
    }

    private Account MapAccount(AccountEntity entity)
        => new(entity.Id,
            entity.Name,
            string.Empty,
            entity.CommodityId,
            (Abstract.AccountType)entity.Type,
            ImmutableList<Account>.Empty);

    private class AccountWithChildren
    {
        public Account Account { get; init; }
        public List<AccountWithChildren> Children { get; set; } = new();

        public string FullName { get; set; }
        public AccountWithChildren Parent { get; set; }
    }
}