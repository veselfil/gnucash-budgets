namespace GnuCashBudget.SharedDomain.Data.Entities;

public class BudgetEntity
{
    public int Id { get; set; }
    public int BudgetedAccountId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Amount { get; set; }
}