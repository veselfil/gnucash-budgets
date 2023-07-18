using System.Collections.Immutable;

namespace GnuCashBudget.GnuCashData.Abstractions.Models;

public record Account(
    string Name,
    string Commodity,
    AccountType AccountType);