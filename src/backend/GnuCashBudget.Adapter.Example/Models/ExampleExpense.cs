namespace GnuCashBudget.Adapter.Example.Models;

public class ExampleExpense
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string Timestamp { get; set; }
    public string? Merchant { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Location { get; set; }
}