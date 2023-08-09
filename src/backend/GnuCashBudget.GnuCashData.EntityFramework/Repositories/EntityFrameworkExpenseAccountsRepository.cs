using System.Collections.Immutable;
using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.Abstractions.Models;

namespace GnuCashBudget.GnuCashData.EntityFramework.Repositories;

public class EntityFrameworkExpenseAccountsRepository: EntityFrameworkAccountTreeBaseRepository, IExpenseAccountsRepository
{
    public EntityFrameworkExpenseAccountsRepository(GnuCashContext context) : base(context)
    {
    }
    
    public async Task<ImmutableList<ExpenseAccount>> GetAllExpenseAccounts()
    {
        var accountsList = FlattenAccountTree(await GetFullAccountTree());

        return accountsList
            .Where(x => x.Account.AccountType == AccountType.Expense)
            .Select(WrapperToDomain)
            .ToImmutableList();
    }

    private ExpenseAccount WrapperToDomain(AccountWithCommodity wrapper)
    {
        return new ExpenseAccount(
            wrapper.Account.Id,
            wrapper.Account.Name,
            wrapper.FullName,
            wrapper.Commodity.Id,
            wrapper.Account.AccountType,
            wrapper.Children.Select(WrapperToDomain).ToImmutableList(),
            wrapper.Commodity.Mnemonic);
    }
}