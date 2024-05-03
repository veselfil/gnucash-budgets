namespace GnuCashBudget.GnuCashData.Abstractions.Models;

public record Transaction(
    string Description,
    DateTime PostDate,
    DateTime EntryDate,
    uint ValueNum,
    uint ValueDenom,
    uint QuantityNum,
    uint QuantityDenom
);