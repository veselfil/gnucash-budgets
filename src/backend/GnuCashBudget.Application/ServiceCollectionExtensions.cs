using GnuCashBudget.Application.Handlers;
using GnuCashBudget.Application.Requests;
using GnuCashBudget.Application.Responses;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GnuCashBudget.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationRequestHandlers(this IServiceCollection self)
    {
        self.AddScoped<IRequestHandler<GetExpenseAccountsRequest, GetExpenseAccountsResponse>,
            GetExpenseAccountsHandler>();

        return self;
    }
}