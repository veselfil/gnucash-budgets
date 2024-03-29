using GnuCashBudget.GnuCashData.Generator.Interfaces;

namespace GnuCashBudget.GnuCashData.Generator.Services;

public sealed class DefaultScopedProcessingService(IGeneratorService generatorService) : IScopedProcessingService
{
    public async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        await generatorService.GenerateExpenses(cancellationToken);
    }
}