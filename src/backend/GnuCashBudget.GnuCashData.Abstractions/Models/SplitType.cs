namespace GnuCashBudget.GnuCashData.Abstractions.Models;

/// <summary>
/// SplitType exists to differentiate between:
/// 1. putting something to the account (credit) = numbs are positiv
/// 2. taking something from the account (debit) = nums are negativ
/// </summary>
public enum SplitType
{
    Credit,
    Debit
}

public static class Extensions
{
    public static int ConvertTypeToInt(this SplitType splitType)
    {
        return splitType == SplitType.Debit ? -1 : 1;
    }
}