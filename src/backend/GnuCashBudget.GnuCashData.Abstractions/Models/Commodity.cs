namespace GnuCashBudget.GnuCashData.Abstractions.Models;

public record Commodity(
    string Id,
    string Namespace,
    string Mnemonic,
    int? Fraction);
    