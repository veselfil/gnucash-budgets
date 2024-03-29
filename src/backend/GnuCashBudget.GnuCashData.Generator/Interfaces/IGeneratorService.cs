namespace GnuCashBudget.GnuCashData.Generator.Interfaces;

public interface IGeneratorService
{
    Task GenerateExpenses(CancellationToken cancellationToken);
}