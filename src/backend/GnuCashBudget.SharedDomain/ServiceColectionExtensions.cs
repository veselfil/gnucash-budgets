using GnuCashBudget.GnuCashData.EntityFramework.Services;
using GnuCashBudget.SharedDomain.GnuCashData.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GnuCashBudget.SharedDomain;

public static class ServiceColectionExtensions
{
    public static IServiceCollection AddSharedDomainServices(this IServiceCollection self)
    {
        return self.AddScoped<IGnuCashAccountsService, GnuCashAccountsService>();
    }
}