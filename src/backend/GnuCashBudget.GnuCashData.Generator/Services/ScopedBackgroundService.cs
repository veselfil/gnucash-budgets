using GnuCashBudget.GnuCashData.Generator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GnuCashBudget.GnuCashData.Generator.Services;

public sealed class ScopedBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ScopedBackgroundService> logger) : BackgroundService
{
    private const string ClassName = nameof(ScopedBackgroundService);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Name} is running", ClassName);

        await DoWorkAsync(cancellationToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Name} is working", ClassName);

        using var scope = serviceScopeFactory.CreateScope();
        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

        await scopedProcessingService.DoWorkAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("{Name} is stopping", ClassName);

        await base.StopAsync(cancellationToken);
    }
}