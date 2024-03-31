using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using GnuCashBudget.GnuCashData.EntityFramework.Helpers;
using Abstract = GnuCashBudget.GnuCashData.Abstractions.Models;
using AccountType = GnuCashBudget.GnuCashData.EntityFramework.Entities.AccountType;

namespace GnuCashBudget.GnuCashData.EntityFramework.Repositories;

public class EntityFrameworkAccountsRepository(GnuCashContext dataContext)
    : EntityFrameworkAccountTreeBaseRepository(dataContext), IAccountRepository
{
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

        return accounts
            .Where(x => x.Account.AccountType == type)
            .Select(WrapperToAccount)
            .ToImmutableList();
    }

    public async Task<(Account?,Commodity?)> GetParentAccountByType(Abstract.AccountType type, bool includeRootAccount, CancellationToken cancellationToken)
    {
        var accountTree = await GetFullAccountTree(includeRootAccount, cancellationToken);
        
        return accountTree
            .Where(x => x.Account.AccountType == type)
            .Select(WrapperToTuple)
            .FirstOrDefault();
    }

    /// <summary>
    /// Method to create an account under some parent account with specified commodity
    /// </summary>
    /// <param name="type">Account type, ROOT, INCOME, EXPENSE, BANK, etc.</param>
    /// <param name="parentId">ID of a parent account. Since we are not creating a ROOT account with this method it can never be NULL</param>
    /// <param name="commodityId">ID of a commodity, got from commodities table</param>
    /// <param name="commodityFraction">Commodity fractions, most commodities (CZK, USD, EUR) have 100 franctions. Only stocks/etfs have 1</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    public async Task<Account> CreateAccount(Abstract.AccountType type, string parentId, string commodityId, int commodityFraction, CancellationToken cancellationToken)
    {
        var accountEntity = MapAccountEntity(type, parentId, commodityId, commodityFraction);
        
        await Context.Accounts.AddAsync(accountEntity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return MapAccount(accountEntity);
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
    
    private (Account,Commodity) WrapperToTuple(AccountWithCommodity wrapper)
    {
        var account = wrapper.Account with
        {
            ChildAccounts = wrapper.Children.Select(WrapperToAccount).ToImmutableList(),
            FullName = wrapper.FullName?.Substring(1) ?? string.Empty,
            Commodity = wrapper.Commodity.Mnemonic
        };

        var commodity = wrapper.Commodity;
        
        return (account, commodity);
    }

    private AccountEntity MapAccountEntity(Abstract.AccountType type, string parentId, string commodityId, int commodityFraction)
    {
        return new AccountEntity
        {
            Id = SimpleHelper.GenerateGuid(),
            Name = $"{type.ToString()}",
            Type = (AccountType)type,
            CommodityId = commodityId,
            CommodityScu = commodityFraction,
            NonStdScu = 0, // TODO Check this out if I can put 0 here for each case
            ParentId = parentId,
            Code = string.Empty,
            Description = $"Generated {type.ToString().ToLower()} account",
            Hidden = false,
            Placeholder = false,
        };
    }
}