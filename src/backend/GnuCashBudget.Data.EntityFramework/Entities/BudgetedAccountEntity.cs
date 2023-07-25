using Microsoft.EntityFrameworkCore;

namespace GnuCashBudget.Data.EntityFramework.Entities;

public class BudgetedAccountEntity
{
    public int Id { get; set; }
    public string AccountGuid { get; set; }
}