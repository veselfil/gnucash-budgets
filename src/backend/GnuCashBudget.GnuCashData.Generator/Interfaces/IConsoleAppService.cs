namespace GnuCashBudget.GnuCashData.Generator.Interfaces;

public interface IConsoleAppService
{
    Task DoWorkAsync(CancellationToken cancellationToken);
}