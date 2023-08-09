using GnuCashBudget.GnuCashData.Abstractions;
using GnuCashBudget.GnuCashData.EntityFramework.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GnuCashBudget.GnuCashData.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGnuCashEntityFrameworkDal(this IServiceCollection self)
    {
        self.AddScoped<IAccountRepository, EntityFrameworkAccountsRepository>();
        self.AddScoped<IAccountTransactionsRepository, EntityFrameworkAccountTransactionsRepository>();
        self.AddScoped<IExpenseAccountsRepository, EntityFrameworkExpenseAccountsRepository>();

        return self;
    }
}