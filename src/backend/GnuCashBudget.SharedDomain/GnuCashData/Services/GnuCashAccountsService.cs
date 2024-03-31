using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions.Models;
using GnuCashBudget.GnuCashData.EntityFramework;
using GnuCashBudget.GnuCashData.EntityFramework.Entities;
using GnuCashBudget.GnuCashData.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Models = GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.SharedDomain.GnuCashData.Services;

public class GnuCashAccountsService: IGnuCashAccountsService
{
    private readonly GnuCashContext _context;

    public GnuCashAccountsService(GnuCashContext context)
    {
        _context = context;
    }

    public async Task<Account?> Find(string accountId)
    {
        var accountTree = await this.GetFullAccountTree();
        var flatList = this.FlattenAccountTree(accountTree);

        return WrapperToAccount(flatList.FirstOrDefault(x => x.Account.Id == accountId));
    }
    
    public async Task<IEnumerable<AccountTransactionView>> GetTransactionsForAccountInDateRange(
        string accountId,
        DateTime from,
        DateTime to)
    {
        var transactions = await _context.Splits
            .Where(x => x.AccountId == accountId)
            .Join(_context.Transactions,
                split => split.TxId,
                transaction => transaction.Id,
                (split, transaction) => new { split, transaction })
            .Join(_context.Accounts,
                x => x.split.AccountId,
                a => a.Id,
                (x, a) => new { x.split, x.transaction, account = a })
            .Where(x => x.transaction.PostDate >= from & x.transaction.PostDate <= to)
            .Select(x =>
                new AccountTransactionViewEntity(x.account.Id, x.account.Name, x.split.QuantityNum, x.split.QuantityDenom,
                    x.transaction.PostDate, x.transaction.Description))
            .ToListAsync();

        return transactions
            .Select(t => t.ToDomainView())
            .ToList();
    }

    public async Task<ImmutableList<Account>> GetAllAccounts()
    {
        var tree = await this.GetFullAccountTree();
        return FlattenAccountTree(tree).Select(WrapperToAccount).ToImmutableList();
    }

    public async Task<ImmutableList<Account>> GetAccountsByType(Models.AccountType type)
    {
        var tree = await this.GetFullAccountTree();
        var accounts = FlattenAccountTree(tree);

        return accounts.Where(x => x.Account.AccountType == type)
            .Select(WrapperToAccount)
            .ToImmutableList();
    }
    
    protected Account WrapperToAccount(AccountWithCommodity wrapper)
    {
        return wrapper.Account with
        {
            ChildAccounts = wrapper.Children.Select(WrapperToAccount).ToImmutableList(),
            FullName = wrapper.FullName.Substring(1),
            Commodity = wrapper.Commodity.Mnemonic
        };
    }

    protected ImmutableList<AccountWithCommodity> FlattenAccountTree(ImmutableList<AccountWithCommodity> accountsTree)
    {
        var accounts = new List<AccountWithCommodity>(accountsTree);
        foreach (var account in accountsTree)
        {
            accounts.AddRange(FlattenAccountTree(account.Children.ToImmutableList()));
        }

        return accounts.ToImmutableList();
    }

    protected async Task<ImmutableList<AccountWithCommodity>> GetFullAccountTree()
    {
        var accounts = await _context.Accounts
            .Join(_context.Commodities, account => account.CommodityId, commodity => commodity.Id,
                (account, commodity) => new { Account = account, Commodity = commodity })
            .ToListAsync();
        
        var rootAccount = accounts.Single(x => x is { Account.Type: GnuCashBudget.GnuCashData.EntityFramework.Entities.AccountType.Root, Account.Name: "Root Account" });
        var rootAccountWrapper = new AccountWithCommodity
        {
            Account = MapAccount(rootAccount.Account),
            Commodity = rootAccount.Commodity
        };

        AssignChildren(rootAccountWrapper);
        BuildFullNames(rootAccountWrapper);

        return rootAccountWrapper.Children
            .ToImmutableList();

        void AssignChildren(AccountWithCommodity account)
        {
            account.Children = accounts
                .Where(x => x.Account.ParentId == account.Account.Id)
                .Select(x => new AccountWithCommodity { Account = MapAccount(x.Account), Commodity = x.Commodity, Parent = account })
                .ToList();

            foreach (var child in account.Children)
            {
                AssignChildren(child);
            }
        }

        void BuildFullNames(AccountWithCommodity account, string name = "")
        {
            if (account.Account.AccountType != Models.AccountType.Root)
                account.FullName = $"{name}:{account.Account.Name}";

            foreach (var childAccount in account.Children)
            {
                BuildFullNames(childAccount, account.FullName);
            }
        }
    }

    private Account MapAccount(AccountEntity entity)
        => new(entity.Id,
            entity.Name,
            "UNKNOWN",
            "UNKNOWN",
            (Models.AccountType) entity.Type,
            ImmutableList<Account>.Empty);

    protected class AccountWithCommodity
    {
        public Account? Account { get; init; }
        
        public CommodityEntity? Commodity { get; init; }
        public List<AccountWithCommodity> Children { get; set; } = new();

        public string? FullName { get; set; }
        public AccountWithCommodity? Parent { get; set; }
    }
    
    private record AccountTransactionViewEntity(string AccountId, string AccountName, int ValueNum, int ValueDenom,
        DateTime PostDate, string Description)
    {
        public AccountTransactionView ToDomainView() =>
            new AccountTransactionView(AccountId, AccountName, Description, (decimal)ValueNum / ValueDenom, PostDate);
    }
}