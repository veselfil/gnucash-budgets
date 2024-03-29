using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
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

    public async Task<(Account?,Commodity?)> GetParentAccountByType(Abstract.AccountType type, bool includeRootAccount)
    {
        var accountTree = await GetFullAccountTree(includeRootAccount);
        
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
    /// <returns></returns>
    public async Task<Account> CreateAccount(Abstract.AccountType type, string parentId, string commodityId, int commodityFraction)
    {
        var accountEntity = new AccountEntity()
        {
            Id = Guid.NewGuid().ToString(),
            Name = $"{type.ToString()} - generated",
            Type = (AccountType)type,
            CommodityId = commodityId,
            CommodityScu = commodityFraction,
            NonStdScu = 0, // TODO Check this out if I can put 0 here for each case
            ParentId = parentId,
            Code = string.Empty,
            Description = "Generated account",
            Hidden = false,
            Placeholder = false,
        };
        
        await Context.Accounts.AddAsync(accountEntity);
        await Context.SaveChangesAsync();

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
    
    (Account,Commodity) WrapperToTuple(AccountWithCommodity wrapper)
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
}