using GnuCashBudget.Adapter.Abstractions.Enums;

namespace GnuCashBudget.Adapter.Abstractions.Models;

public class Expense
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string Timestamp { get; set; }
    public string? Merchant { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? Location { get; set; }
}