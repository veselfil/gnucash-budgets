namespace GnuCashBudget.GnuCashData.Generator.Interfaces;

public interface IScopedProcessingService
{
    Task DoWorkAsync(CancellationToken cancellationToken = default);
}